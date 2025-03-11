//
// Created by regha on 04.03.2025.
//

#ifndef GAME_H
#define GAME_H
#include <array>
#include <iostream>
#include <memory>
#include <random>
#include <GL/gl.h>
#include "GameViewModel.h"


constexpr int GAME_FIELD_WIDTH_VIEW = 12;
constexpr int GAME_FIELD_HEIGHT_VIEW = 21;



class CellView // todo можно переделать на стратегию
{
public:
	CellView()
		: m_color(0.0f, 0.0f, 0.0f) {}

	CellView(int x, int y, int size, Color color)
		: m_x(x), m_y(y), m_size(size), m_color(color) {}

	virtual ~CellView() = default;

	[[nodiscard]] virtual bool isTetrino() const { return false; };
	[[nodiscard]] virtual bool isBarrier() const { return false; };

	void Draw() const
	{
		DrawImpl();
	}

protected:
	virtual void DrawImpl() const {};

	Color m_color;
	int m_x = 0, m_y = 0;
	int m_size = 0;
};

class BarrierCellView : public CellView
{
public:
	BarrierCellView(int x, int y, int size)
		: CellView(x, y, size, BARRIER_COLOR) {}

	bool isTetrino() const override { return false; }
	bool isBarrier() const override { return true; }

	void DrawImpl() const override
	{
		glBegin(GL_QUADS);
		glColor3f(m_color.r(), m_color.g(), m_color.b());
		glVertex2d(m_x, m_y);
		glVertex2d(m_x, m_y + m_size);
		glVertex2d(m_x + m_size, m_y + m_size);
		glVertex2d(m_x + m_size, m_y);
		glEnd();
	}

};

class EmptyCellGameView : public CellView
{
public:
	EmptyCellGameView(int x, int y, int size)
		: CellView(x, y, size, EMPTY_CELL_COLOR) {}

	bool isTetrino() const override { return false; }
	bool isBarrier() const override { return false; }

	void DrawImpl() const override
	{
		glBegin(GL_QUADS);
		glColor3f(m_color.r(), m_color.g(), m_color.b());
		glVertex2d(m_x, m_y);
		glVertex2d(m_x, m_y + m_size);
		glVertex2d(m_x + m_size, m_y + m_size);
		glVertex2d(m_x + m_size, m_y);
		glEnd();
	}
};

class TetrinoCellView : public CellView
{
public:
	TetrinoCellView(int x, int y, int size, Color color)
		: CellView(x, y, size, color) {}

	bool isTetrino() const override { return true; }
	bool isBarrier() const override { return false; }

	void DrawImpl() const override
	{
		//обводка
		glBegin(GL_QUADS);
		glColor3f(m_color.r(), m_color.g(), m_color.b());
		glVertex2d(m_x, m_y);
		glVertex2d(m_x, m_y + m_size);
		glVertex2d(m_x + m_size, m_y + m_size);
		glVertex2d(m_x + m_size, m_y);
		glEnd();
		glLineWidth(1.5f);
		glBegin(GL_LINES);
		glColor3f(BORDER_TETRINO_COLOR.r(), BORDER_TETRINO_COLOR.g(), BORDER_TETRINO_COLOR.b());

		glVertex2d(m_x, m_y);
		glVertex2d(m_x, m_y + m_size);

		glVertex2d(m_x, m_y + m_size);
		glVertex2d(m_x + m_size, m_y + m_size);

		glVertex2d(m_x + m_size, m_y + m_size);
		glVertex2d(m_x + m_size, m_y);

		glVertex2d(m_x + m_size, m_y);
		glVertex2d(m_x, m_y);
		glEnd();
		glLineWidth(1.0f);
		//конец обводки
	}
};


class GameView
{
public:
	GameView(): m_viewModel(std::make_unique<GameViewModel>())
	{
		toView();
		m_viewModel->start();
	}


	void Draw()
	{
		MoveEveryThreeSeconds();
		toView();
		for (int row = 0; row < GAME_FIELD_HEIGHT_VIEW; ++row)
		{
			for (int col = 0; col < GAME_FIELD_WIDTH_VIEW; ++col)
			{
				m_gameField[row][col]->Draw();
			}
		}
	}

	~GameView()
	{
		for (int row = 0; row < GAME_FIELD_HEIGHT_VIEW; ++row)
		{
			for (int col = 0; col < GAME_FIELD_WIDTH_VIEW; ++col)
			{
				m_gameField[row][col]->~CellView();
			}
		}
	}

	void onKeyPressed(int key) {
		m_viewModel->onKeyPressed(key);
	}

private:

	void toView()
	{
		auto cells = m_viewModel->getCells();
		int startY = 10;
		int cellSize = 20;
		for (int row = 0; row < GAME_FIELD_HEIGHT_VIEW; ++row)
		{
			startY += cellSize;
			int startX = 50;
			for (int col = 0; col < GAME_FIELD_WIDTH_VIEW; ++col)
			{
				startX += cellSize;
				if (row == 0)
				{
					m_gameField[row][col] = std::make_unique<BarrierCellView>(startX, startY, cellSize);
					continue;
				}
				if (col == 0 || col == GAME_FIELD_WIDTH_VIEW - 1)
				{
					m_gameField[row][col] = std::make_unique<BarrierCellView>(startX, startY, cellSize);
					continue;
				}

				if (row > cells.size())
				{
					continue;
				}
				auto cell = cells[row - 1][col - 1];
				if (cell->isTetrino())
				{
					m_gameField[row][col] = std::make_unique<TetrinoCellView>(startX, startY, cellSize, cell->getColor());
				}
				else
				{
					m_gameField[row][col] = std::make_unique<EmptyCellGameView>(startX, startY, cellSize);
				}
			}
		}
	}

	std::unique_ptr<CellView> m_gameField[GAME_FIELD_HEIGHT_VIEW][GAME_FIELD_WIDTH_VIEW];
	void MoveEveryThreeSeconds() {
		auto now = std::chrono::steady_clock::now();
		auto elapsed = std::chrono::duration_cast<std::chrono::seconds>(now - m_lastMoveTime).count();

		if (elapsed >= 1) {
			std::cout << "отрисовка" << std::endl;
			m_viewModel->update();
			m_lastMoveTime = now;
		}

	}

	bool m_isGameStarting = false;
	int m_score = START;
	int level = START;
	int speed = START_SPEED;
	std::unique_ptr<GameViewModel> m_viewModel;

	std::chrono::steady_clock::time_point m_lastMoveTime;
};


#endif //GAME_H