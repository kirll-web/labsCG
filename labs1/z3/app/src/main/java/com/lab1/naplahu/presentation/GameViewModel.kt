package com.lab1.naplahu.presentation


import androidx.compose.runtime.State
import androidx.compose.runtime.mutableStateOf
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.lab1.naplahu.TypeDialog
import com.lab1.naplahu.model.Letter
import com.lab1.naplahu.model.LetterInWord
import com.lab1.naplahu.model.LetterState
import kotlinx.coroutines.flow.MutableSharedFlow
import kotlinx.coroutines.flow.asSharedFlow
import kotlinx.coroutines.launch

data class GameState(
    val letters: List<Letter>,
    val word: List<LetterInWord>,
    val numbersOfAttempts: Int,
    val typeDialog: TypeDialog
) {
    companion object {
        fun default() = GameState(
            emptyList(),
            emptyList(),
            0,
            TypeDialog.NOT_SHOW_TYPE
        )
    }
}

sealed interface GameEvent {
    data class onClickLetter(val letter: Letter) : GameEvent
}

sealed class UIEvent {
    data class ShowToast(val message: String) : UIEvent()
    data object ShowFailGame : UIEvent()
}

class GameViewModel() : ViewModel() {
    private val mWords = listOf(
        "Привет",
        "Корабль",
        "Кот",
        "Дирижабль",
        "Спички",
        "Зубочистка",
        "Хорошо",
        "Жвачка",
        "Спирт",
        "Телефон",
        "Смартфон"
    )
    private val mLetters = listOf(
        'А',
        'Б',
        'В',
        'Г',
        'Д',
        'Е',
        'Ё',
        'Ж',
        'З',
        'И',
        'Й',
        'К',
        'Л',
        'М',
        'Н',
        'О',
        'П',
        'Р',
        'С',
        'Т',
        'У',
        'Ф',
        'Х',
        'Ц',
        'Ч',
        'Ш',
        'Щ',
        'Ъ',
        'Ы',
        'Ь',
        'Э',
        'Ю',
        'Я'
    )

    private val mState = mutableStateOf(GameState.default())
    private val mEvents = MutableSharedFlow<UIEvent>()

    val events = mEvents.asSharedFlow()
    val state: State<GameState> = mState

    init {
        startGame()
    }


    fun event(event: GameEvent) {
        when (event) {
            is GameEvent.onClickLetter -> onClickLetter(event.letter)
        }
    }

    private fun onClickLetter(letter: Letter) {
        if (letter.state !is LetterState.NotUsedLetter) return

        when (state.value.word.find { it.value.lowercase() == letter.value.lowercase() }) {
            null -> {
                mState.value = mState.value.copy(
                    letters = state.value.letters.map {
                        if (it.value.lowercase() == letter.value.lowercase()) it.copy(state = LetterState.IncorrectLetter)
                        else it
                    },
                    numbersOfAttempts = state.value.numbersOfAttempts - 1
                )
            }

            else -> mState.value = mState.value.copy(
                word = state.value.word.map {
                    if (it.value.lowercase() == letter.value.lowercase()) it.copy(isVisible = true)
                    else it
                },
                letters = state.value.letters.map {
                    if (it.value.lowercase() == letter.value.lowercase()) it.copy(state = LetterState.CorrectLetter)
                    else it
                },
            )
        }

        if (state.value.word.all { it.isVisible }) {
            mState.value = mState.value.copy(
                typeDialog = TypeDialog.WIN_TYPE
            )
        }

        if (state.value.numbersOfAttempts == 0) {
            mState.value = mState.value.copy(
                typeDialog = TypeDialog.FAIL_TYPE
            )
        }
    }

    fun restart() {
        startGame()
    }

    private fun startGame() {
        val randomWord = mWords.random()

        val indexes = randomWord.mapIndexed { index, _ -> index }
        val shuffeledIndexed = indexes.shuffled()

        mState.value = mState.value.copy(
            word = randomWord.mapIndexed { index, it ->
                when (index) {
                    shuffeledIndexed[0],
                    shuffeledIndexed[1] -> LetterInWord(it, true)

                    else -> LetterInWord(it, false)
                }
            },
            letters = mLetters.map { Letter(it, LetterState.NotUsedLetter) },
            numbersOfAttempts = MAX_ATTEMPTS,
            typeDialog = TypeDialog.NOT_SHOW_TYPE
        )
    }

    private fun sendUIEvent(uiEvent: UIEvent) {
        viewModelScope.launch {
            mEvents.emit(uiEvent)
        }
    }

    companion object {
        private val MAX_ATTEMPTS = 6
    }
}



