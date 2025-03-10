package com.lab1.lab2.z1.ui

import android.graphics.ImageDecoder
import android.net.Uri
import android.os.Build
import android.util.Log
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.annotation.RequiresApi
import androidx.compose.foundation.Image
import androidx.compose.foundation.gestures.detectDragGestures
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.offset
import androidx.compose.foundation.layout.size
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.ImageBitmap
import androidx.compose.ui.graphics.asImageBitmap
import androidx.compose.ui.input.pointer.pointerInput
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.platform.LocalConfiguration
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.unit.dp
import androidx.lifecycle.viewmodel.compose.viewModel
import com.lab1.lab2.ui.CommonButton
import com.lab1.lab2.z1.presentation.Z1ViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext

@RequiresApi(Build.VERSION_CODES.R)
@Composable
fun Z1Screen(
    viewModel: Z1ViewModel = viewModel()
) {
    val state by viewModel.state
    val context = LocalContext.current
    val bitmap = remember(state.image?.uri) { mutableStateOf<ImageBitmap?>(null) }

    val configuration = LocalConfiguration.current
    val screenWidth = configuration.screenWidthDp.toDouble()
    val screenHeight = configuration.screenHeightDp.toDouble()

    val orientation = remember(configuration) {
        configuration.orientation
    }

    LaunchedEffect(key1 = orientation) {
        viewModel.updateImg(state.image?.uri, screenWidth, screenHeight, context)
    }

    LaunchedEffect(key1 = state.image?.uri) {
        state.image?.let { image ->
            bitmap.value = withContext(Dispatchers.IO) {
                try {
                    val source = ImageDecoder.createSource(context.contentResolver, image.uri)
                    ImageDecoder.decodeBitmap(source).asImageBitmap()
                } catch (e: Exception) { null }
            }
        } ?: run {
            bitmap.value = null
        }
    }
    val launcher = rememberLauncherForActivityResult(
        contract =
        ActivityResultContracts.GetContent()
    ) { uri: Uri? ->
        viewModel.updateImg(uri, screenWidth, screenHeight, context)
    }
    Box(modifier = Modifier.fillMaxSize()) {
        Column(
            modifier = Modifier.fillMaxSize(),
            horizontalAlignment = Alignment.Start,
            verticalArrangement = Arrangement.Top
        ) {
            Row(
                modifier = Modifier.fillMaxWidth()
            ) {
                CommonButton(text = "Open") {
                    launcher.launch("image/*")
                }
            }
        }

        bitmap.value?.let { btm ->
            state.image?.let { image ->
                Image(
                    modifier = Modifier
                        .offset(
                            image.x.dp,
                            image.y.dp
                        )
                        .size(
                            image.width.dp,
                            image.height.dp
                        )
                        .pointerInput(Unit) {
                            detectDragGestures { change, dragAmount ->
                                change.consume()
                                viewModel.updateImgCoords(
                                    (state.image?.x ?: 0.0) + dragAmount.x,
                                    (state.image?.y ?: 0.0) + dragAmount.y
                                )
                            }
                        },
                    bitmap = btm,
                    contentDescription = null,
                    contentScale = ContentScale.Crop,
                )
            }
        }
    }
}
