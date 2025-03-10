//
// Created by regha on 28.02.2025.
//

#ifndef APP_H
#define APP_H
#include "../Game/GameView.h"

#include <vector>
#include <GL/gl.h>
#include <chrono>


class App {
public:
	App() :m_game(GameView()){}

	~App()
	{
		m_quads.clear();
	}

	void Draw() {
		glClearColor(1.0f, 1.0f, 1.0f, 1.0f);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
		glBegin(GL_QUADS);
		glColor3f(1.0f, 0.0f, 0.0f);
		if (m_quads.size() > 1) {
			auto it = m_quads[0];
			for (size_t i = 1; i < m_quads.size(); i++) {
				glVertex2d(it.first, it.second);
				glVertex2d(m_quads[i].first, m_quads[i].second);
				it = m_quads[i];
			}
			glVertex2d(it.first, it.second);
			glVertex2d(m_quads[0].first, m_quads[0].second);
		}
		glEnd();

		m_game.Draw();
	}

private:
	GameView m_game;
	std::vector<std::pair<float, float>> m_quads = {
		{10, 10},
		{10, 20},
		{20, 20},
		{20, 10},
	};
};


#endif //APP_H
