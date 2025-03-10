package model

import kotlinx.coroutines.flow.StateFlow

interface IModel {
    val shapes: StateFlow<List<Shape>>
    fun changeShapeCoords(shapeId: Int, x: Double, y: Double)
}