package com.lab1.lab2.z1.presentation

import android.content.Context
import android.graphics.BitmapFactory
import android.media.ExifInterface
import android.net.Uri
import androidx.compose.runtime.State
import androidx.compose.runtime.mutableStateOf
import androidx.lifecycle.ViewModel
import kotlin.math.min

data class Image(
    val uri: Uri,
    val x: Double,
    val y: Double,
    val width: Double,
    val height: Double
)

data class Z1State(
    val image: Image? = null
)

class Z1ViewModel : ViewModel() {
    private val mState = mutableStateOf(Z1State())
    val state: State<Z1State> = mState

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
                val scaledWidth = if (scale < 1.0) imageWidthDp  * scale else imageWidthDp
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

    fun updateImgCoords(
        x: Double,
        y: Double
    ) {
        mState.value.image?.let {
            mState.value = mState.value.copy(
                image = it.copy(x = x, y = y)
            )
        }

    }

    private fun calculateScaleFactor(
        imageWidth: Double,
        imageHeight: Double,
        screenWidth: Double,
        screenHeight: Double
    ): Pair<Double, Double> {
        if (imageWidth <= screenWidth && imageHeight <= screenHeight) {
            return  1.0 to  1.0
        }

        val widthRatio = screenWidth / imageWidth
        val heightRatio = screenHeight / imageHeight
        return widthRatio to heightRatio
    }
}