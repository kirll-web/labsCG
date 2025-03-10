package com.lab1.naplahu.model


sealed interface LetterState {
    object CorrectLetter: LetterState
    object IncorrectLetter: LetterState

    object NotUsedLetter: LetterState
}

data class Letter(
    val value: Char,
    val state: LetterState = LetterState.NotUsedLetter
) {
}

data class LetterInWord(
    val value: Char,
    val isVisible: Boolean
) {
}