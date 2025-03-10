package com.lab1.lab2.z2.data

data class Color(
    val name: String,
    val value: Long
)
class Data {

    private val mColors = mapOf(
        "Green" to Color("Green", 0xFF00FF00),
        "Black" to Color("Black", 0xFF000000),
        "Red" to Color("Red", 0xFFFF0000),
        "Blue" to Color("Blue", 0xFF0000FF),
        "Yellow" to Color("Yellow", 0xFFFFFF00),
        "Cyan" to Color("Cyan", 0xFF00FFFF)
    )

    fun getColors() = mColors

}