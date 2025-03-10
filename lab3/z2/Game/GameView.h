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

class Color
{
public:
	Color(float r, float g, float b)
		: m_r(r), m_g(g), m_b(b) {}

	float r() const { return m_r; }
	float g() const { return m_g; }
	float b() const { return m_b; }

private:
	float m_r;
	float m_g;
	float m_b;
};


constexpr int GAME_FIELD_WIDTH = 12;
constexpr int GAME_FIELD_HEIGHT = 21;
const Color BARRIER_COLOR = Color(0.0, 0.0, 0.0);
const Color EMPTY_CELL_COLOR = Color(1.0, 1.0, 1.0);
const Color BORDER_TETRINO_COLOR = Color(0.0, 0.0, 0.0);
constexpr int COUNT_LINES_ON_LEVEL = 5;
constexpr int START = 0;
constexpr int START_SPEED = 10;


class Cell // todo можно переделать на стратегию
{
public:
	Cell()
		: m_color(0.0f, 0.0f, 0.0f) {}

	Cell(int x, int y, int size, Color color)
		: m_x(x), m_y(y), m_size(size), m_color(color) {}

	virtual ~Cell() = default;

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

class BarrierCell : public Cell
{
public:
	BarrierCell(int x, int y, int size)
		: Cell(x, y, size, BARRIER_COLOR) {}

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

class EmptyCellGame : public Cell
{
public:
	EmptyCellGame(int x, int y, int size)
		: Cell(x, y, size, EMPTY_CELL_COLOR) {}

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

class TetrinoCell : public Cell
{
public:
	TetrinoCell(int x, int y, int size, Color color)
		: Cell(x, y, size, color) {}

	bool isTetrino() const override { return true; }
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

		//fixme mock
		//обводка

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


class Tetramino
{
public:
	Tetramino(int startX, int startY, int size)
		: cells(generateCells(startX, startY, size)) {}

private:
	std::array<TetrinoCell, 4> generateCells(int startX, int startY, int size) const
	{
		static std::random_device rd;
		static std::mt19937 gen(rd());

		std::uniform_int_distribution<> dist(0, static_cast<int>(Type::O));

		auto randomType = static_cast<Type>(dist(gen));
		std::array<TetrinoCell, 4> cells{}; // Создаём массив ячеек
		switch (randomType)
		{
		case Type::O:
			break;
		case Type::L:
			break;
		case Type::Z:
			break;
		case Type::I:
			break;
		case Type::T:
			break;
		}

		return cells;
	}

	enum class Type
	{
		L, Z, T, I, O
	};

	std::array<TetrinoCell, 4> cells;
};

class GameView
{
public:
	GameView()
	{
		int startY = 10;
		int cellSize = 20;
		for (int row = 0; row < GAME_FIELD_HEIGHT; ++row)
		{
			startY += cellSize;
			int startX = 50;
			for (int col = 0; col < GAME_FIELD_WIDTH; ++col)
			{
				startX += cellSize;
				if (row == 0)
				{
					m_gameField[row][col] = std::make_unique<BarrierCell>(startX, startY, cellSize);
					continue;
				}
				if (col == 0 || col == GAME_FIELD_WIDTH - 1)
				{
					m_gameField[row][col] = std::make_unique<BarrierCell>(startX, startY, cellSize);
					continue;
				}

				m_gameField[row][col] = std::make_unique<EmptyCellGame>(startX, startY, cellSize);
			}
		}
	}

	void Draw() const
	{
		for (int row = 0; row < GAME_FIELD_HEIGHT; ++row)
		{
			for (int col = 0; col < GAME_FIELD_WIDTH; ++col)
			{
				m_gameField[row][col]->Draw();
			}
		}
	}

	~GameView()
	{
		for (int row = 0; row < GAME_FIELD_HEIGHT; ++row)
		{
			for (int col = 0; col < GAME_FIELD_WIDTH; ++col)
			{
				m_gameField[row][col]->~Cell();
			}
		}
	}

private:
	std::unique_ptr<Cell> m_gameField[GAME_FIELD_HEIGHT][GAME_FIELD_WIDTH];
	bool m_isGameStarting = false;
	int m_score = START;
	int level = START;
	int speed = START_SPEED;

};


#endif //GAME_H