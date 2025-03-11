// #include <glad/glad.h>
#include "FuncParabola.h"
#include "GLFWInitializer.h"
#include "Window.h"

constexpr int WINDOWS_SIZE_X = 640;
constexpr int WINDOWS_SIZE_y = 480;
constexpr int MAX_X = 20;
constexpr int MIN_X = -MAX_X;
constexpr int MAX_Y = 20;
constexpr int MIN_Y = -MAX_Y;
constexpr int X_MIN_FUNC = -2;
constexpr int X_MAX_FUNC = 3;
//при маленьких размерах окна изображение должно вписываться
int main()
{
	GLFWInitializer initGLFW;
	auto func = FuncParabola(X_MIN_FUNC, X_MAX_FUNC);

	Window window
	{
		WINDOWS_SIZE_X,
		WINDOWS_SIZE_y,
		MIN_X,
		MAX_X,
		MIN_Y,
		MAX_Y,
		"Hello, parabola",
		App(&func),
	};
	window.Run();

	return 0;
}