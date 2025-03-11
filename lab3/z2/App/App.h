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
		m_game.Draw();
	}

	void onKeyPressed(int key) {
		m_game.onKeyPressed(key);
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
