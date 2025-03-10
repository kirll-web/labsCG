package com.lab1.lab2.utils.navigation

sealed class NavigationRoute(val route: String) {
    data object Menu: NavigationRoute("menu")
    data object Z1: NavigationRoute("z1")
    data object Z2: NavigationRoute("z2")
    data object Z3: NavigationRoute("z3")
}