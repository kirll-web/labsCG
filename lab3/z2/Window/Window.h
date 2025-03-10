//
// Created by regha on 28.02.2025.
//

#include "BaseWindow.h"
#include "../App/App.h"

class Window : public BaseWindow
{
public:
	Window(int width, int height, int xMin, int xMax, int yMin, int yMax, const char* title, App &app)
		: BaseWindow(
			  width,
			  height,
			  xMin,
			  xMax,
			  yMin, yMax,
			  title
			  ),
		  app(app),
		  m_xMin(xMin),
		  m_yMin(yMin),
		  m_xMax(xMax),
		  m_yMax(yMax) {}

private:
	void Draw(int width, int height) override
	{
		app.Draw();
	}

	App &app;
	int m_xMin;
	int m_xMax;
	int m_yMin;
	int m_yMax;
};