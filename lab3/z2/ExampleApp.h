//
// Created by regha on 28.02.2025.
//

#ifndef APP_H
#define APP_H
#include <vector>
#include <GL/gl.h>

#include "Func.h"

#include <chrono>


class App {
public:
	App() {
		m_lastMoveTime = std::chrono::steady_clock::now();
	}

	void Draw() {
		glClearColor(1.0f, 1.0f, 1.0f, 1.0f);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

		glColor3f(0.0f, 0.0f, 0.0f); // Черный цвет

		glBegin(GL_QUADS);
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

		MoveEveryThreeSeconds();
	}

private:
	std::vector<std::pair<float, float>> m_quads = {
		{-10, 0},
		{-10, 10},
		{10, 10},
		{10, 0},
	};

	std::chrono::steady_clock::time_point m_lastMoveTime;

	void MoveEveryThreeSeconds() {
		auto now = std::chrono::steady_clock::now();
		auto elapsed = std::chrono::duration_cast<std::chrono::seconds>(now - m_lastMoveTime).count();

		if (elapsed >= 3) {
			for (auto& quad : m_quads) {
				quad.second -= 1; // Смещаем по оси Y на -1
			}
			m_lastMoveTime = now;
		}
	}
};


#endif //APP_H
