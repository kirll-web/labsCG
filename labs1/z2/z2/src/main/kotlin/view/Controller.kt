package view

import model.IModel

class Controller(
    private val model: IModel
): IController {
    override fun changeCoords(id: Int, x: Double, y: Double) {
        model.changeShapeCoords(id, x, y)
    }
}