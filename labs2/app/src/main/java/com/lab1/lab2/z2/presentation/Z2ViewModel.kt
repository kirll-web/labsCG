package com.lab1.lab2.z2.presentation

import android.content.ContentValues
import android.content.Context
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.graphics.Paint
import android.graphics.Rect
import android.media.ExifInterface
import android.net.Uri
import android.os.Build
import android.os.Environment
import android.provider.MediaStore
import android.util.Log
import androidx.annotation.RequiresApi
import androidx.compose.runtime.State
import androidx.compose.runtime.mutableStateOf
import androidx.compose.ui.graphics.ImageBitmap
import androidx.compose.ui.graphics.Path
import androidx.compose.ui.graphics.asAndroidBitmap
import androidx.compose.ui.graphics.asAndroidPath
import androidx.compose.ui.unit.Density
import androidx.compose.ui.unit.IntSize
import androidx.compose.ui.unit.dp
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewModelScope
import com.lab1.lab2.z2.data.Data
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlin.math.min

data class Image(
    val uri: Uri,
    val x: Double,
    val y: Double,
    val width: Double,
    val height: Double
)

data class PathData(
    val path: Path,
    val color: ColorView
)

data class Z2State(
    val image: Image? = null,
    val pathData: List<PathData> = emptyList(),
    val colors: Map<String, ColorView> = mapOf(),
    val selectedColor: SelectedColor = SelectedColor("Black", 0x000000)
)

data class ColorView(
    val name: String,
    val value: Long
)


data class SelectedColor(
    val name: String,
    val value: Long
)

class Z2ViewModelFactory : ViewModelProvider.Factory {
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(Z2ViewModel::class.java)) {
            @Suppress("UNCHECKED_CAST")
            return Z2ViewModel(Data()) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}

class Z2ViewModel(
    private val data: Data
) : ViewModel() {
    private val mState = mutableStateOf(Z2State())
    val state: State<Z2State> = mState

    init {
        mState.value = mState.value.copy(colors = data.getColors().map { (key, value) ->
            key to ColorView(name = value.name, value = value.value)
        }.toMap())
        val selectedColorView = try {
            mState.value.colors.values.toList()[0]
        } catch (ex: Exception) {
            null
        }
        selectedColorView?.let {
            selectColor(selectedColorView.name)
        }
    }

    fun updateImg(
        img: Uri?,
        screenWidth: Double,
        screenHeight: Double,
        context: Context
    ) {
        img?.let {
            context.contentResolver.openInputStream(img)?.use { inputStream ->
                val options = BitmapFactory.Options().apply { inJustDecodeBounds = true }
                BitmapFactory.decodeStream(inputStream, null, options)
                inputStream.close()
                val exif = ExifInterface(context.contentResolver.openInputStream(img)!!)

                val orientation = exif.getAttributeInt(
                    ExifInterface.TAG_ORIENTATION,
                    ExifInterface.ORIENTATION_NORMAL
                )

                val (widthPx, heightPx) = when (orientation) {
                    ExifInterface.ORIENTATION_ROTATE_90,
                    ExifInterface.ORIENTATION_ROTATE_270 -> {
                        options.outHeight.toDouble() to options.outWidth.toDouble()
                    }

                    else -> {
                        options.outWidth.toDouble() to options.outHeight.toDouble()
                    }
                }

                val density = context.resources.displayMetrics.density
                val imageWidthDp = widthPx / density
                val imageHeightDp = heightPx / density

                val scaleFactor = calculateScaleFactor(
                    imageWidth = imageWidthDp,
                    imageHeight = imageHeightDp,
                    screenWidth = screenWidth,
                    screenHeight = screenHeight
                )
                val scale = min(scaleFactor.first, scaleFactor.second)
                val scaledWidth = if (scale < 1.0) imageWidthDp * scale else imageWidthDp
                val scaledHeight = if (scale < 1.0) imageHeightDp * scale else imageHeightDp

                val centerX = (screenWidth - scaledWidth) / 2
                val centerY = (screenHeight - scaledHeight) / 2
                mState.value = mState.value.copy(
                    image = Image(
                        uri = img,
                        x = centerX,
                        y = centerY,
                        width = scaledWidth,
                        height = scaledHeight
                    )
                )
            }

        }
    }

    fun selectColor(name: String) {
        mState.value = mState.value.copy(selectedColor = mState.value.colors[name]?.let {
            SelectedColor(it.name, it.value)
        } ?: SelectedColor("Black", 0x000000))
    }

    fun clearPath() {
        mState.value = mState.value.copy(pathData = emptyList())
    }

    fun addPath(path: PathData) {
        mState.value = mState.value.copy(pathData = mState.value.pathData.plus(path))
    }

    @RequiresApi(35)
    fun removeLastPath() {
        mState.value = mState.value.copy(
            pathData = mState.value.pathData.toMutableList().apply { removeLast() })
    }

    private fun calculateScaleFactor(
        imageWidth: Double,
        imageHeight: Double,
        screenWidth: Double,
        screenHeight: Double
    ): Pair<Double, Double> {
        if (imageWidth <= screenWidth && imageHeight <= screenHeight) {
            return 1.0 to 1.0
        }

        val widthRatio = screenWidth / imageWidth
        val heightRatio = screenHeight / imageHeight
        return widthRatio to heightRatio
    }

    @RequiresApi(Build.VERSION_CODES.Q)
    fun save(canvasSize: IntSize, bitmapState: ImageBitmap?, density: Density, context: Context) {
        viewModelScope.launch(Dispatchers.IO) {
            canvasSize.let { size ->
                if (size.width > 0 && size.height > 0) {
                    val bitmap = Bitmap.createBitmap(
                        size.width,
                        size.height,
                        Bitmap.Config.ARGB_8888
                    )

                    val canvas = android.graphics.Canvas(bitmap)
                    canvas.drawColor(android.graphics.Color.WHITE)

                    val p = Paint()
                    p.strokeWidth = 7f
                    p.style = Paint.Style.STROKE

                    bitmapState?.let { btm ->
                        mState.value.image?.let {
                            with(density) {
                                val widthPx = it.width.dp.toPx()
                                val heightPx = it.height.dp.toPx()

                                canvas.drawBitmap(
                                    btm.asAndroidBitmap()
                                        .copy(android.graphics.Bitmap.Config.ARGB_8888, true),
                                    null,
                                    Rect(
                                        0,
                                        0,
                                        widthPx.toInt(),
                                        heightPx.toInt()
                                    ),
                                    Paint()
                                )
                            }
                        }
                    }
                    state.value.pathData.forEach {
                        p.color = it.color.value.toInt()
                        canvas.drawPath(
                            it.path.asAndroidPath(),
                            p
                        )
                    }
                    saveBitmapToGallery(context, bitmap)
                }
            }
        }
    }

    private fun saveBitmapToGallery(context: Context, bitmap: Bitmap) {
        try {
            val contentValues = ContentValues().apply {
                put(
                    MediaStore.MediaColumns.DISPLAY_NAME,
                    "canvas_${System.currentTimeMillis()}.png"
                )
                put(MediaStore.MediaColumns.MIME_TYPE, "image/png")
                if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.Q) {
                    put(MediaStore.MediaColumns.RELATIVE_PATH, Environment.DIRECTORY_PICTURES)
                }
            }

            context.contentResolver.insert(
                MediaStore.Images.Media.EXTERNAL_CONTENT_URI,
                contentValues
            )?.let { uri ->
                context.contentResolver.openOutputStream(uri)?.use { stream ->
                    bitmap.compress(Bitmap.CompressFormat.PNG, 100, stream)
                }
            }
        } catch (e: Exception) {
            Log.e("SaveBitmap", "Error: ${e.message}")
        }
    }
}