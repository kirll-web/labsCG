//
// Created by regha on 28.02.2025.
//

#ifndef APP_H
#define APP_H
#include <vector>
#include <GL/gl.h>

#include "Func.h"


class App {
public:
    App(Func* func): m_func(func) {}

    void Draw() {
    	glClearColor(1.0f, 1.0f, 1.0f, 1.0f);
    	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);


        glColor3f(0.0f, 0.0f, 0.0f); // Черный цвет
        glBegin(GL_LINES);

        // Ось X
        glVertex2d(-20, 0);
        glVertex2d(20, 0);

        // Ось Y
        glVertex2d(0, -20);
        glVertex2d(0, 20);

        glEnd();

    	m_func->Draw();

    }
private:
    Func* m_func;
};


#endif //APP_H
