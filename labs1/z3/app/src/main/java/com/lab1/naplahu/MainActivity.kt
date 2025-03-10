package com.lab1.naplahu

import android.os.Bundle
import android.util.Log
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.Canvas
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.ExperimentalLayoutApi
import androidx.compose.foundation.layout.FlowRow
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.layout.wrapContentHeight
import androidx.compose.material3.AlertDialog
import androidx.compose.material3.Button
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.remember
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.geometry.Offset
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.TextStyle
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.lifecycle.viewmodel.compose.viewModel
import com.lab1.naplahu.model.Letter
import com.lab1.naplahu.model.LetterInWord
import com.lab1.naplahu.model.LetterState
import com.lab1.naplahu.presentation.GameEvent
import com.lab1.naplahu.presentation.GameViewModel
import com.lab1.naplahu.ui.theme.NaPlahuTheme

enum class TypeDialog {
    WIN_TYPE,
    FAIL_TYPE,
    NOT_SHOW_TYPE
}

class MainActivity() : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            NaPlahuTheme {
                Surface(
                    modifier = Modifier.fillMaxSize(),
                    color = MaterialTheme.colorScheme.background
                ) {
                    Game()
                }
            }
        }
    }
}

@OptIn(ExperimentalLayoutApi::class)
@Composable
fun Game(
    gameViewModel: GameViewModel = viewModel()
) {
    val state by gameViewModel.state

    Column {
        FlowRow(
            modifier = Modifier.fillMaxWidth().wrapContentHeight(align = Alignment.Bottom),
            horizontalArrangement = Arrangement.Center,
            verticalArrangement = Arrangement.Bottom
        ) {
            state.word.forEach {
                LetterInWordView(it)
            }
        }

        FlowRow(
            modifier = Modifier.fillMaxWidth(),
            horizontalArrangement = Arrangement.Center,
        ) {
            state.letters.forEach {
                LetterView(it) { letter ->
                    gameViewModel.event(GameEvent.onClickLetter(letter))
                }
            }
        }
        HangmanView(
            state.numbersOfAttempts,
            modifier = Modifier
                .weight(1f)
        )

        AlertDialog(state.typeDialog) {
            gameViewModel.restart()
        }
    }
}

//todo добавить модель
@Composable
fun HangmanView(
    errors: Int,
    modifier: Modifier = Modifier
) {
    Canvas(
        modifier = Modifier
            .fillMaxSize()
    ) {
        val width = size.width
        val height = size.height

        // Основание виселицы
        drawLine(
            Color.Black,
            Offset(10f, height - 10),
            Offset(width - 10f, height - 10f),
            strokeWidth = 8f
        )
        drawLine(
            Color.Black,
            Offset(width * 0.4f, 10f),
            Offset(width * 0.4f, height - 10),
            strokeWidth = 8f
        )
        drawLine(
            Color.Black,
            Offset(width * 0.4f, 10f),
            Offset(width * 0.7f, 10f),
            strokeWidth = 8f
        )
        drawLine(
            Color.Black,
            Offset(width * 0.7f, 10f),
            Offset(width * 0.7f, 100f),
            strokeWidth = 4f
        )

        // Голова
        if (errors <= 5) drawCircle(
            Color.Black,
            30f,
            Offset(width * 0.7f, 120f)
        )

        // Тело
        if (errors <= 4) drawLine(
            Color.Black,
            Offset(width * 0.7f, 130f),
            Offset(width * 0.7f, 170f),
            strokeWidth = 6f
        )

        // Руки
        if (errors <= 3) {
            drawLine(
                Color.Black,
                Offset(width * 0.7f, 150f),
                Offset(width * 0.65f, 160f),
                strokeWidth = 6f
            )
        }

        // Руки
        if (errors <= 2) {
            drawLine(
                Color.Black,
                Offset(width * 0.7f, 150f),
                Offset(width * 0.75f, 160f),
                strokeWidth = 6f
            )
        }

        // Ноги
        if (errors <= 1) {
            drawLine(
                Color.Black,
                Offset(width * 0.7f, 170f),
                Offset(width * 0.65f, 180f),
                strokeWidth = 6f
            )
        }
        // Ноги
        if (errors == 0) {
            drawLine(
                Color.Black,
                Offset(width * 0.7f, 170f),
                Offset(width * 0.75f, 180f),
                strokeWidth = 6f
            )
        }
    }
}

@Composable
fun AlertDialog(
    typeDialog: TypeDialog,
    onClick: () -> Unit
) {
    if (typeDialog == TypeDialog.NOT_SHOW_TYPE) return

    AlertDialog(
        onDismissRequest = {},
        confirmButton = {
            Button(onClick = { onClick() }) {
                Text("Рестарт")
            }
        },
        text = {
            Text(
                when (typeDialog) {
                    TypeDialog.FAIL_TYPE -> "Вы проиграли"
                    TypeDialog.WIN_TYPE -> "Вы выиграли"
                    else -> ""
                }
            )
        },
        dismissButton = null
    )

}

@Composable
fun LetterInWordView(
    letterInWord: LetterInWord
) {
    Column(
        modifier = Modifier
            .width(40.dp)
            .padding(10.dp),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.Bottom
    ) {
        Text(
            text = letterInWord.value.toString(),
            style = TextStyle(
                when {
                    letterInWord.isVisible -> Color.Black
                    else -> Color.Transparent
                },
                30.sp,
                FontWeight.Bold
            )
        )
        Spacer(modifier = Modifier.height(7.dp))
        Box(
            modifier = Modifier
                .fillMaxWidth()
                .height(3.dp)
                .background(Color.Black)
        )
    }
}


@Composable
fun LetterView(
    letter: Letter,
    onClick: (Letter) -> Unit
) {
    Column(
        modifier = Modifier
            .padding(5.dp)
            .clickable { onClick(letter) },
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Text(
            modifier = Modifier.padding(10.dp),
            text = letter.value.toString(),
            style = TextStyle(
                when (letter.state) {
                    LetterState.NotUsedLetter -> Color.Black
                    LetterState.CorrectLetter -> Color.Green
                    LetterState.IncorrectLetter -> Color.Red
                },
                20.sp,
                FontWeight.Bold
            )
        )
    }
}

@Preview(showBackground = true)
@Composable
fun HangmanViewPreview() {
    HangmanView(
        errors = 3
    )
}
