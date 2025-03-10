package com.lab1.lab2.ui.menu

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material3.Button
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.navigation.NavController
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import com.lab1.lab2.utils.navigation.NavigationRoute
import org.intellij.lang.annotations.JdkConstants.HorizontalAlignment

@Composable
fun MenuScreen(
    navController: NavController
) {

    Column(
        modifier = Modifier.fillMaxSize(),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.Center
    ) {
        Button(onClick = { navController.navigate(NavigationRoute.Z1.route) }) {
            Text(text = "Задание 1")
        }

        Button(
            enabled = true,
            onClick = { navController.navigate(NavigationRoute.Z2.route) }
        ) {
            Text(text = "Задание 2")
        }

        Button(
            enabled = false,
            onClick = { navController.navigate(NavigationRoute.Z3.route) }
        ) {
            Text(text = "Задание 3")
        }
    }
}