#include <stdexcept>
#include "GLFW/glfw3.h"

#include <iostream>

class BaseWindow
{
public:
	BaseWindow(int w, int h, int xMin, int xMax, int yMin, int yMax, const char* title)
		: m_window{ CreateGLFWWindow(w, h, title) },
		  m_xMin(xMin),
		  m_xMax(xMax),
		  m_yMin(yMin),
		  m_yMax(yMax),
		  m_windowSizeX(w),
		  m_windowSizeY(h)
	{
		if (!m_window)
			throw std::runtime_error("Failed to create window");
		glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 4);
		glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 6);
		glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_COMPAT_PROFILE);
		glfwMakeContextCurrent(m_window);
		glfwSetWindowUserPointer(m_window, this);
		glfwSetWindowRefreshCallback(m_window, &BaseWindow::RefreshCallback);
		glfwSetKeyCallback(m_window, &BaseWindow::KeyCallback);
		glfwSetCursorPosCallback(m_window, &BaseWindow::CursorPosCallback);
		glfwSetWindowSizeCallback(m_window, &BaseWindow::WindowSizeCallback);
		glMatrixMode(GL_MODELVIEW);
		glLoadIdentity();
		glOrtho(
			xMin,
			xMax,
			yMin,
			yMax,
			-1,
			11
			);
		glMatrixMode(GL_MODELVIEW);
	}

	BaseWindow(const BaseWindow&) = delete; // Запрещаем копирование
	BaseWindow& operator=(const BaseWindow&) = delete; // Запрещаем присваивание

	virtual ~BaseWindow()
	{
		glfwDestroyWindow(m_window);
	}

	void Run()
	{
		while (!glfwWindowShouldClose(m_window))
		{
			int w, h;
			glfwGetFramebufferSize(m_window, &w, &h);
			Draw(w, h);
			glfwSwapBuffers(m_window);
			glfwPollEvents();
		}
	}

protected:
	virtual void Draw(int width, int height) = 0;
	virtual void onKeyPressed(int key) {}
	virtual void OnRefresh() {}
	virtual void OnCursorPos([[maybe_unused]] double x, [[maybe_unused]] double y) {} // Пустая реализация по умолчанию

private:
	static GLFWwindow* CreateGLFWWindow(int w, int h, const char* title)
	{
		return glfwCreateWindow(w, h, title, nullptr, nullptr);
	}

	static BaseWindow* GetInstance(GLFWwindow* window)
	{
		return static_cast<BaseWindow*>(glfwGetWindowUserPointer(window));
	}

	static void RefreshCallback(GLFWwindow* window)
	{
		if (auto instance = GetInstance(window))
			instance->OnRefresh();
	}

	static void WindowSizeCallback(GLFWwindow* window, int width, int height)
	{}

	static void KeyCallback(GLFWwindow* window, int key, int scancode, int action, int mods)
	{
		if (auto instance = GetInstance(window))
		{
			if (action == GLFW_RELEASE)
			{
				instance->onKeyPressed(key);
			}
		}
	}

	static void CursorPosCallback(GLFWwindow* window, double x, double y)
	{
		if (auto instance = GetInstance(window))
			instance->OnCursorPos(x, y);
	}

	GLFWwindow* m_window;
	int m_windowSizeX, m_windowSizeY;
	int m_xMin, m_xMax;
	int m_yMin, m_yMax;
};