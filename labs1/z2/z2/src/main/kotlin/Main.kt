import androidx.compose.material.MaterialTheme
import androidx.compose.ui.window.Window
import androidx.compose.ui.window.application
import model.Model
import view.Controller
import view.View


fun main() = application {
    val model = Model()
    val controller = Controller(model)
    val view = View(model, controller)
    //нарисовать view

    Window(onCloseRequest = ::exitApplication) {
        MaterialTheme {
            view.draw()
        }
    }
}
