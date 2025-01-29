//
// Created by regha on 29.01.2025.
//

#ifndef MODEL_H
#define MODEL_H
#include <vector>

class Line {
    Line(const int x, const int y): cX(x), cY(y) {}

    int cX;
    int cY;
};

class Letter {
public:
    Letter(std::vector<Line> lines)
        : lines(std::move(lines)) {}

    std::vector<Line> lines;
};



#endif //MODEL_H
