// #include <glad/glad.h>
#include "App/App.h"
#include "Core/GLFWInitializer.h"
#include "Window/Window.h"

constexpr int WINDOWS_SIZE_X = 640;
constexpr int WINDOWS_SIZE_y = 480;
constexpr int MAX_X = 640;
constexpr int MIN_X = 0;
constexpr int MAX_Y = 480;
constexpr int MIN_Y = 0;


int main(void)
{
	GLFWInitializer initGLFW;
	auto app = App();
	Window window
	{
		WINDOWS_SIZE_X,
		WINDOWS_SIZE_y,
		MIN_X,
		MAX_X,
		MIN_Y,
		MAX_Y,
		"Hello, triangle",
		app
	};
	window.Run();

	return 0;
}