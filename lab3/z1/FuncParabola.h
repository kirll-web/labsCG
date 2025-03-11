//
// Created by regha on 28.02.2025.
//

#ifndef FUNCPARABOLA_H
#define FUNCPARABOLA_H
#include <vector>
#include <GL/gl.h>

#include "Func.h"


class FuncParabola : public Func {
public:
    FuncParabola(double xMinFunc, double xMaxFunc): Func(xMinFunc, xMaxFunc) {
        for (double x = xMinFunc; x < xMaxFunc; x += 0.01) {
            m_points.emplace_back(x, CalculateY(x));
        }
    }


protected:
    void DrawImpl() override {
        glBegin(GL_LINES);

        if (m_points.size() > 1) {
            auto it = m_points[0];
            for (auto i = 1; i < m_points.size(); i++) {
                glVertex2d(it.first, it.second);
                glVertex2d(m_points[i].first, m_points[i].second);
                it = m_points[i];
            }
        }
        for (auto point : m_points) {
            glVertex2d(point.first, point.second);
        }

        glEnd();
    }

private:
    static double CalculateY(double x) {
        return 2 * x * x - 3 * x - 8;
    }

    std::vector<std::pair<double, double>> m_points;
};


#endif //FUNCPARABOLA_H
