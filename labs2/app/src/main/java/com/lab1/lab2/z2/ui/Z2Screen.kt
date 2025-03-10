package com.lab1.lab2.z2.ui

import android.content.ContentValues
import android.content.Context
import android.graphics.Bitmap
import android.graphics.ImageDecoder
import android.graphics.Paint
import android.graphics.Rect
import android.net.Uri
import android.os.Build
import android.os.Environment
import android.provider.MediaStore
import android.util.Log
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.annotation.RequiresApi
import androidx.compose.foundation.Canvas
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.gestures.detectDragGestures
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.ExperimentalLayoutApi
import androidx.compose.foundation.layout.FlowRow
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.drawWithContent
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.ImageBitmap
import androidx.compose.ui.graphics.Path
import androidx.compose.ui.graphics.asAndroidBitmap
import androidx.compose.ui.graphics.asAndroidPath
import androidx.compose.ui.graphics.asImageBitmap
import androidx.compose.ui.graphics.copy
import androidx.compose.ui.graphics.drawscope.Stroke
import androidx.compose.ui.graphics.drawscope.clipRect
import androidx.compose.ui.input.pointer.pointerInput
import androidx.compose.ui.layout.onSizeChanged
import androidx.compose.ui.platform.LocalConfiguration
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.platform.LocalDensity
import androidx.compose.ui.unit.IntSize
import androidx.compose.ui.unit.dp
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.lifecycle.viewmodel.viewModelFactory
import com.lab1.lab2.ui.CommonButton
import com.lab1.lab2.z2.presentation.ColorView
import com.lab1.lab2.z2.presentation.PathData
import com.lab1.lab2.z2.presentation.SelectedColor
import com.lab1.lab2.z2.presentation.Z2ViewModel
import com.lab1.lab2.z2.presentation.Z2ViewModelFactory
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext

@RequiresApi(Build.VERSION_CODES.Q)
@Composable
fun Z2Screen() {
    Column(
        modifier = Modifier.fillMaxSize()
    ) {
        DrawableCanvas()
    }
}

@RequiresApi(Build.VERSION_CODES.Q)
@Composable
fun DrawableCanvas(
    viewModel: Z2ViewModel = viewModel(factory = Z2ViewModelFactory())
) {
    val context = LocalContext.current
    var tempPath = Path()
    val canvasSize = remember { mutableStateOf(IntSize.Zero) }
    val state by viewModel.state
    val bitmapState = remember(state.image?.uri) { mutableStateOf<ImageBitmap?>(null) }
    val density = LocalDensity.current
    val configuration = LocalConfiguration.current
    val screenWidth = configuration.screenWidthDp.toDouble()
    val screenHeight = configuration.screenHeightDp.toDouble()

    val orientation = remember(configuration) {
        configuration.orientation
    }

    val modifier = remember {
        mutableStateOf<Modifier>(Modifier)
    }

    LaunchedEffect(key1 = orientation) {
        viewModel.updateImg(state.image?.uri, screenWidth, screenHeight, context)
    }

    LaunchedEffect(key1 = state.image?.uri) {
        viewModel.clearPath()
        tempPath = Path()
        state.image?.let { image ->
            bitmapState.value = withContext(Dispatchers.IO) {
                try {
                    val source = ImageDecoder.createSource(context.contentResolver, image.uri)
                    modifier.value = Modifier
                        .width(state.image?.width?.dp ?: 0.dp)
                        .height(state.image?.height?.dp ?: 0.dp)
                    viewModel.clearPath()
                    tempPath = Path()
                    ImageDecoder.decodeBitmap(source).asImageBitmap()
                } catch (e: Exception) {
                    modifier.value = Modifier.fillMaxSize()
                    viewModel.clearPath()
                    tempPath = Path()
                    null
                }
            }
        } ?: run {
            viewModel.clearPath()
            tempPath = Path()
            modifier.value = Modifier.fillMaxSize()
            bitmapState.value = null
        }
    }
    val launcher = rememberLauncherForActivityResult(
        contract =
        ActivityResultContracts.GetContent()
    ) { uri: Uri? ->
        viewModel.updateImg(uri, screenWidth, screenHeight, context)
    }
    Column {
        Row(
            modifier = Modifier.fillMaxWidth()
        ) {
            CommonButton("Сохранить в PNG") {
                viewModel.save(canvasSize.value, bitmapState.value, density, context)
            }
            CommonButton(text = "Open") {
                launcher.launch("image/*")
            }
        }
        Colors(
            state.colors.values.toList(),
            state.selectedColor,
            viewModel::selectColor
        )

        Canvas(
            modifier = modifier.value
                .background(Color.White)
                .pointerInput(true) {
                    detectDragGestures(onDragStart = { tempPath = Path()}, onDragEnd = {
                        viewModel.addPath(PathData(tempPath, ColorView(state.selectedColor.name, state.selectedColor.value)))
                    }) { change, dragAmount ->
                        tempPath.moveTo(
                            change.position.x - dragAmount.x,
                            change.position.y - dragAmount.y
                        )
                        tempPath.lineTo(change.position.x, change.position.y)
                        if (state.pathData.isNotEmpty()) viewModel.removeLastPath()
                        viewModel.addPath(PathData(tempPath, ColorView(state.selectedColor.name, state.selectedColor.value)))
                    }
                }
                .drawWithContent {
                    clipRect {
                        bitmapState.value?.let { btm ->
                            state.image?.let {
                                with(density) {
                                    val widthPx = it.width.dp.toPx()
                                    val heightPx = it.height.dp.toPx()

                                    drawImage(
                                        btm,
                                        srcSize = IntSize(btm.width, btm.height),
                                        dstSize = IntSize(widthPx.toInt(), heightPx.toInt())
                                    )
                                }
                            }
                        }
                        state.pathData.forEach {
                            drawPath(
                                it.path,
                                color = Color(it.color.value),
                                style = Stroke(7f)
                            )
                        }
                    }
                }
                .onSizeChanged { size ->
                    canvasSize.value = size
                }
        ) {}
    }
}

@OptIn(ExperimentalLayoutApi::class)
@Composable
fun Colors(
    colors: List<ColorView>,
    selectedColor: SelectedColor,
    selectColor: (String) -> Unit
) {
    FlowRow(modifier = Modifier.fillMaxWidth()) {
        colors.forEach {
            Box(
                modifier = Modifier
                    .background(
                        color = when {
                            it.name == selectedColor.name -> Color.Black
                            else -> Color.Transparent
                        }, shape = CircleShape
                    )
                    .size(42.dp)
            ) {
                Box(
                    modifier = Modifier
                        .background(color = Color(it.value), shape = CircleShape)
                        .clickable { selectColor(it.name) }
                        .size(40.dp)
                )
            }

        }
    }
}

