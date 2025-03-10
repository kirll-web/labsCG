package view

import androidx.compose.foundation.gestures.detectDragGestures
import androidx.compose.foundation.layout.offset
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.geometry.Offset
import androidx.compose.ui.input.pointer.pointerInput
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.IntOffset
import kotlinx.coroutines.*
import kotlinx.coroutines.flow.launchIn
import kotlinx.coroutines.flow.onEach
import model.IModel
import model.Image
import model.Shape
import kotlin.math.roundToInt
import androidx.compose.foundation.Image as ComposeImage

class View(
    private val model: IModel,
    private val controller: IController
) {
    private var mShapes = mutableStateListOf<Shape>()
    private val scope = CoroutineScope(Job() + Dispatchers.Default)

    init {
        scope.launch {
            delay(500)
            model.shapes.onEach { newShapes ->
                mShapes.clear()
                mShapes.addAll(newShapes)
            }.launchIn(this)
        }
    }

    @Composable
    fun draw() {
        for (shape in mShapes) {
            when (shape) {
                is Image -> {
                    var offset by remember {
                        mutableStateOf(
                            Offset(
                                shape.x.toFloat(),
                                shape.y.toFloat()
                            )
                        )
                    }

                    ComposeImage(
                        painter = painterResource(shape.getSourse()),
                        contentDescription = null,
                        modifier = Modifier
                            .offset { IntOffset(offset.x.roundToInt(), offset.y.roundToInt()) }
                            .pointerInput(Unit) {
                                detectDragGestures { change, dragAmount ->
                                    change.consume()
                                    offset =
                                        Offset(offset.x + dragAmount.x, offset.y + dragAmount.y)
                                    controller.changeCoords(
                                        shape.id,
                                        offset.x.toDouble(),
                                        offset.y.toDouble()
                                    )
                                }
                            }
                    )
                }
            }
        }
    }
}