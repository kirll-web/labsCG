#include <stdexcept>
#include "GLFW/glfw3.h"

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
		glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_COMPAT_PROFILE);
		glfwMakeContextCurrent(m_window);
		glfwSetWindowUserPointer(m_window, this);
		glfwSetWindowRefreshCallback(m_window, &BaseWindow::RefreshCallback);
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

	BaseWindow(const BaseWindow&) = delete;
	BaseWindow& operator=(const BaseWindow&) = delete;

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
			Draw();
			glfwSwapBuffers(m_window);
			glfwPollEvents();
		}
	}

protected:
	virtual void Draw() = 0;

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
	{
		if (auto instance = GetInstance(window))
		{
			instance->m_windowSizeX = width;
			instance->m_windowSizeY = height;
			glViewport(0, 0, width, height);
			glMatrixMode(GL_MODELVIEW);
			glLoadIdentity();

			if (float aspect = width / height; aspect > 1.0f)
			{
				const float widthScale = (instance->m_xMax - instance->m_xMin) * aspect;
				const float centerX = (instance->m_xMax + instance->m_xMin) / 2.0f;
				glOrtho(centerX - widthScale / 2, centerX + widthScale / 2,
						instance->m_yMin, instance->m_yMax, -1, 1);
			}
			else
			{
				const float heightScale = (instance->m_yMax - instance->m_yMin) / aspect;
				const float centerY = (instance->m_yMax + instance->m_yMin) / 2.0f;
				glOrtho(instance->m_xMin, instance->m_xMax,
						centerY - heightScale / 2, centerY + heightScale / 2, -1, 1);
			}

			glMatrixMode(GL_MODELVIEW);
		}
	}

	static void CursorPosCallback(GLFWwindow* window, double x, double y)
	{
		if (auto instance = GetInstance(window))
			instance->OnCursorPos(x, y);
	}


	virtual void OnRefresh() {}
	virtual void OnCursorPos([[maybe_unused]] double x, [[maybe_unused]] double y) {} // Пустая реализация по умолчанию

	GLFWwindow* m_window;
	int m_windowSizeX, m_windowSizeY;
	int m_xMin, m_xMax;
	int m_yMin, m_yMax;
};