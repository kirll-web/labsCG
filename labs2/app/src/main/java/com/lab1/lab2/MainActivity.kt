package com.lab1.lab2

import android.os.Build
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.annotation.RequiresApi
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.ui.Modifier
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import com.lab1.lab2.ui.menu.MenuScreen
import com.lab1.lab2.ui.theme.Lab2Theme
import com.lab1.lab2.utils.navigation.NavigationRoute
import com.lab1.lab2.z1.ui.Z1Screen
import com.lab1.lab2.z2.ui.Z2Screen
import com.lab1.lab2.z3.ui.Z3Screen

class MainActivity : ComponentActivity() {
    @RequiresApi(Build.VERSION_CODES.R)
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            Lab2Theme {
                Surface(
                    modifier = Modifier.fillMaxSize(),
                    color = MaterialTheme.colorScheme.background
                ) {
                    val navController = rememberNavController()

                    NavHost(navController = navController, startDestination = "menu") {

                        composable(NavigationRoute.Menu.route) { MenuScreen(navController) }

                        composable(NavigationRoute.Z1.route) { Z1Screen() }

                        composable(NavigationRoute.Z2.route) { Z2Screen() }

                        composable(NavigationRoute.Z3.route) { Z3Screen() }
                    }
                }
            }
        }
    }

}