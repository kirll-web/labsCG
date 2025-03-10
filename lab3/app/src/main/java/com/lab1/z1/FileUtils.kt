package com.lab1.z1

import android.content.Context
import android.content.res.Resources
import java.io.BufferedReader
import java.io.IOException
import java.io.InputStreamReader


class FileUtils {
   companion object {
       fun readTextFromRaw(context: Context, resourceId: Int): String {
           val stringBuilder = StringBuilder()
           try {
               var bufferedReader: BufferedReader? = null
               try {
                   val inputStream = context.resources.openRawResource(resourceId)
                   bufferedReader = BufferedReader(InputStreamReader(inputStream))
                   var line: String?
                   while (bufferedReader.readLine().also { line = it } != null) {
                       stringBuilder.append(line)
                       stringBuilder.append("\r\n")
                   }
               } finally {
                   bufferedReader?.close()
               }
           } catch (ex: IOException) {
               ex.printStackTrace()
           } catch (ex: Resources.NotFoundException) {
               ex.printStackTrace()
           }
           return stringBuilder.toString()
       }
   }
}