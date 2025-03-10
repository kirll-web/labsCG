package model

import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow


//todo привести в порядок архитектуру
class Model: IModel {
    private var mShapes = MutableStateFlow<List<Shape>>(emptyList())
    override val shapes: StateFlow<List<Shape>> = mShapes

    init {
        mShapes.value = mShapes.value.plus(Image(0, 111.0,111.0,"drawable/rocket.svg"))
            .plus(Image(0, 211.0,211.0,"drawable/rocket.svg"))
    }

    override fun changeShapeCoords(shapeId: Int, x: Double, y: Double) {
        mShapes.value = mShapes.value.map { shape ->
            if (shape.id == shapeId) shape.prototype(x, y) else shape
        }
    }
}

abstract class Shape(
    val id: Int,
    val x: Double,
    val y: Double
)  {
    abstract fun prototype(x: Double, y: Double): Shape
}

class Image(
    id: Int,
    x: Double,
    y: Double,
    private val source: String
) : Shape(id, x ,y) {
    fun getSourse() = source
    override fun prototype(x: Double, y: Double): Shape {
        return Image(id, x, y, source)
    }
}