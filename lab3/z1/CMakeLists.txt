cmake_minimum_required(VERSION 3.10)
set(PROJECT_NAME z1)
project("${PROJECT_NAME}")

set(CMAKE_CXX_STANDARD 20)
add_executable(z1 main.cpp
        GLFWInitializer.cpp
        GLFWInitializer.h
        BaseWindow.cpp
        BaseWindow.h
        Window.cpp
        Window.h
        App.cpp
        App.h
        FuncParabola.cpp
        FuncParabola.h
        Func.cpp
        Func.h)

set(GLFW_BUILD_EXAMPLES OFF CACHE BOOL "" FORCE)
set(GLFW_BUILD_TESTS OFF CACHE BOOL "" FORCE)
set(GLFW_BUILD_DOC OFF CACHE BOOL "" FORCE)
set(GLFW_INSTALL OFF CACHE BOOL "" FORCE)


target_link_libraries(${PROJECT_NAME} -lopengl32)
add_subdirectory(external/glfw)
target_link_libraries(${PROJECT_NAME} glfw)
