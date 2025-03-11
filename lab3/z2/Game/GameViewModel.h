//
// Created by regha on 09.03.2025.
//

#ifndef GAMEVIEWMODEL_H
#define GAMEVIEWMODEL_H


//
// Created by regha on 04.03.2025.
//


#include <array>
#include <chrono>
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


constexpr int GAME_FIELD_WIDTH = 10;
constexpr int GAME_FIELD_HEIGHT = 20;
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
		: m_x(0), m_y(0), m_color(0.0f, 0.0f, 0.0f) {}

	Cell(int x, int y, Color color)
		: m_x(x), m_y(y), m_color(color) {}

	Cell(const Cell& other)
		: m_x(other.m_x), m_y(other.m_y), m_color(other.m_color) {}

	int getX() const { return m_x; }
	int getY() const { return m_y; }
	Color getColor() { return m_color; }

	virtual ~Cell() = default;

	virtual bool isTetrino() const { return false; };

protected:
	Color m_color;
	int m_x, m_y;
};


class EmptyCellGame : public Cell
{
public:
	EmptyCellGame(int x, int y)
		: Cell(x, y, EMPTY_CELL_COLOR) {}

	bool isTetrino() const override { return false; }
};

class TetrinoCell : public Cell
{
public:
	TetrinoCell(int x, int y, Color color)
		: Cell(x, y, color) {}


	void changeCoords(int offsetX, int offsetY)
	{
		m_x += offsetX;
		m_y += offsetY;
	}

	bool isTetrino() const override { return true; }
};

/* TODO мысль такая:
TODO делаем сначала модель игры с генерацией тетрамино. Без отрисовки
TODO Просто в координаты x,y(номера ячеек поля) подставляем нужные модели клеток.
TODO А во view уже будем задавать логику отрисовки.

TODO Что это даст:
TODO легче будет переворачивать
TODO не надо запариваться сильно с координатами и высчитывать их
TODO должно получиться что то вроде
0 0 0 0 1 0 0 0
0 0 0 1 1 0 0 0
0 0 0 0 1 0 0 0
0 0 0 0 0 0 0 0
0 0 0 0 0 0 0 0
0 0 0 0 0 0 0 0
0 0 0 0 0 0 1 0
0 0 0 0 0 0 1 1
0 0 0 0 0 0 0 1
0 0 0 0 1 1 1 1
*/

class Tetramino
{
public:
	Tetramino(int startX, int startY)
		: m_cells(generateCells(startX, startY)) {}

	const std::array<TetrinoCell, 4>& getCells() const
	{
		return m_cells;
	}

	void changeCoords(int offsetX, int offsetY)
	{
		for (int i = 0; i < m_cells.size(); i++)
		{
			m_cells.at(i).changeCoords(offsetX, offsetY);
		}
	}

private:
	static std::array<TetrinoCell, 4> generateCells(int startX, int startY)
	{
		int min = 0;
		int max = 4;

		// Инициализируем генератор случайных чисел
		std::random_device rd;
		std::mt19937 gen(rd());

		// Создаём объект uniform_int_distribution<>
		std::uniform_int_distribution<> distrib(min, max);

		// Генерируем случайное число в диапазоне [min, max]
		int randomValue = distrib(gen);
		switch (randomValue)
		{
		case 0:
			return std::array<TetrinoCell, 4>({
				TetrinoCell(startX, startY, Color(1.0, 0.0, 0.0)),
				TetrinoCell(startX, startY + 1, Color(1.0, 0.0, 0.0)),
				TetrinoCell(startX + 1, startY + 1, Color(1.0, 0.0, 0.0)),
				TetrinoCell(startX + 1, startY, Color(1.0, 0.0, 0.0)) });
		case 1:
			return std::array<TetrinoCell, 4>({
				TetrinoCell(startX, startY, Color(1.0, 0.0, 0.0)),
				TetrinoCell(startX + 1, startY, Color(1.0, 0.0, 0.0)),
				TetrinoCell(startX, startY + 1, Color(1.0, 0.0, 0.0)),
				TetrinoCell(startX, startY + 2, Color(1.0, 0.0, 0.0)) });
		case 2:
			return std::array<TetrinoCell, 4>({
				TetrinoCell(startX, startY + 1, Color(1.0, 0.0, 0.0)),
				TetrinoCell(startX + 1, startY + 1, Color(1.0, 0.0, 0.0)),
				TetrinoCell(startX + 1, startY, Color(1.0, 0.0, 0.0)),
				TetrinoCell(startX + 2, startY, Color(1.0, 0.0, 0.0)) });
		case 3:
			return std::array<TetrinoCell, 4>({
				TetrinoCell(startX, startY, Color(1.0, 0.0, 0.0)),
				TetrinoCell(startX, startY + 1, Color(1.0, 0.0, 0.0)),
				TetrinoCell(startX, startY + 2, Color(1.0, 0.0, 0.0)),
				TetrinoCell(startX, startY + 3, Color(1.0, 0.0, 0.0)) });
		case 4:
			return std::array<TetrinoCell, 4>({
				TetrinoCell(startX, startY, Color(1.0, 0.0, 0.0)),
				TetrinoCell(startX + 1, startY, Color(1.0, 0.0, 0.0)),
				TetrinoCell(startX + 2, startY, Color(1.0, 0.0, 0.0)),
				TetrinoCell(startX + 1, startY + 1, Color(1.0, 0.0, 0.0)) });
		default:
			throw std::invalid_argument("Invalid type");
		}

	}

	enum class Type
	{
		L, Z, T, I, O
	};

	std::array<TetrinoCell, 4> m_cells;
};

class GameViewModel
{
public:
	GameViewModel()
	{
		for (int row = 0; row < GAME_FIELD_HEIGHT; ++row)
		{
			for (int col = 0; col < GAME_FIELD_WIDTH; ++col)
			{
				m_gameField[row][col] = std::make_unique<EmptyCellGame>(row, col);
			}
		}
	}

	GameViewModel(GameViewModel& other)
	{
		m_gameField = other.m_gameField;
		m_nextTetramino = std::make_unique<Tetramino>(*other.m_nextTetramino);
		m_currentTetramino = std::make_unique<Tetramino>(*other.m_currentTetramino);
		m_isGameStarting = other.m_isGameStarting;
		m_score = other.m_score;
		level = other.level;
		speed = other.speed;
		m_lastMoveTime = other.m_lastMoveTime;
	}

	~GameViewModel()
	{
		for (int row = 0; row < GAME_FIELD_HEIGHT; ++row)
		{
			for (int col = 0; col < GAME_FIELD_WIDTH; ++col)
			{
				m_gameField[row][col]->~Cell();
			}
		}
	}

	void start()
	{
		m_isGameStarting = true;
		m_currentTetramino = std::make_unique<Tetramino>(Tetramino(GAME_FIELD_WIDTH / 2, GAME_FIELD_HEIGHT));
		m_nextTetramino = std::make_unique<Tetramino>(Tetramino(GAME_FIELD_WIDTH / 2, GAME_FIELD_HEIGHT));
	}

	void update()
	{
		continueGame();
	}

	void onKeyPressed(int key)
	{
		if (key == 263)
		{
			changeCoords(-1, 0);
		}
		if (key == 262)
		{
			changeCoords(1, 0);
		}
		if (key == 264)
		{
			changeCoords(0, -1);
		}
		std::cout << key << std::endl;
	}

	const std::array<std::array<std::shared_ptr<Cell>, GAME_FIELD_WIDTH>, GAME_FIELD_HEIGHT> & getCells() const
	{
		return m_gameField;
	}

private:
	void changeCoords(int offsetX, int offsetY)
	{
		if (m_isTetraminoStopped)
		{
			// ReSharper disable once CppDFAUnreachableCode
			m_currentTetramino = std::move(m_nextTetramino);
			m_nextTetramino = std::make_unique<Tetramino>(Tetramino(GAME_FIELD_WIDTH / 2, GAME_FIELD_HEIGHT));
			m_isTetraminoStopped = false;
			return;
		}

		auto cells = m_currentTetramino->getCells();
		int minY = INFINITY;
		for (auto it : cells)
		{
			if (it.getY() < GAME_FIELD_HEIGHT)
			{
				if (minY > it.getY())
					minY = it.getY();
				if (it.getY() > minY)
					continue;
				if (it.getY() == 0 || m_gameField[it.getY() - 1][it.getX()]->isTetrino())
				{
					m_isTetraminoStopped = true;
					break;
				}
			}
		}

		if (!m_isTetraminoStopped)
		{
			auto cells = m_currentTetramino->getCells();
			m_currentTetramino->changeCoords(offsetX, offsetY);
			auto newCells = m_currentTetramino->getCells();
			for (int i = 0; i < cells.size(); i++)
			//todo добавить проверку на x,y,  потому что они могут выходить за края
			{
				if (cells[i].getY() < GAME_FIELD_HEIGHT)
				{
					m_gameField[cells[i].getY()][cells[i].getX()].reset();
					m_gameField[cells[i].getY()][cells[i].getX()] = std::make_shared<EmptyCellGame>(cells[i].getX(), cells[i].getY());
				}
				if (newCells[i].getY() < GAME_FIELD_HEIGHT)
				{
					m_gameField[newCells[i].getY()][newCells[i].getX()].reset();
					m_gameField[newCells[i].getY()][newCells[i].getX()] = std::make_shared<TetrinoCell>(
						newCells[i].getX(),
						newCells[i].getY(),
						newCells[i].getColor()
						);
				}
				else
				{
					if (cells[i].getY() < GAME_FIELD_HEIGHT)
					{
						m_gameField[cells[i].getY()][cells[i].getX()].reset();
						m_gameField[cells[i].getY()][newCells[i].getX()] = std::make_shared<TetrinoCell>(
							cells[i].getX(),
							cells[i].getY(),
							cells[i].getColor()
							);
					}
				}
			}
		}

	}

	void continueGame()
	{
		if (m_isGameStarting)
		{
			changeCoords(0, -1);
		}
	}


	std::array<std::array<std::shared_ptr<Cell>, GAME_FIELD_WIDTH>, GAME_FIELD_HEIGHT> m_gameField;
	std::unique_ptr<Tetramino> m_nextTetramino;
	std::unique_ptr<Tetramino> m_currentTetramino;
	bool m_isGameStarting = false;
	bool m_isTetraminoStopped = false;
	int m_score = START;
	int level = START;
	int speed = START_SPEED;
	std::chrono::steady_clock::time_point m_lastMoveTime;
};


#endif //GAMEVIEWMODEL_H