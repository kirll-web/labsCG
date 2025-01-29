#include <cmath>
#include <SFML/Graphics.hpp>

#include "cmake-build-debug/_deps/sfml-src/include/SFML/Graphics/ConvexShape.hpp"


sf::ConvexShape createCircle(float radius, sf::Vector2f position, int pointCount, sf::Color color) {
    sf::ConvexShape semiCircle;
    semiCircle.setPointCount(pointCount + 2); // +2: центр и последняя точка

    semiCircle.setPoint(0, sf::Vector2f(0, radius)); // Центральная точка

    for (int i = 0; i <= pointCount; ++i) {
        float angle = (-90.0f + (i * 180.0f / pointCount)) * (3.14159265f / 180.0f); // Радианы
        float x = radius * std::cos(angle);
        float y = radius * std::sin(angle) + radius;
        semiCircle.setPoint(i + 1, sf::Vector2f(x, y));
    }

    semiCircle.setOutlineColor(color);
    semiCircle.setOutlineThickness(10);
    semiCircle.setPosition(position);

    return semiCircle;
}

int main() {
    auto color = sf::Color::Blue;
    auto transparentColor  = sf::Color::Transparent;

    /*Д*/
    sf::ConvexShape triangleD;
    triangleD.setPointCount(3);
    triangleD.setPoint(0, sf::Vector2f(100, 100));
    triangleD.setPoint(1, sf::Vector2f(50, 250));
    triangleD.setPoint(2, sf::Vector2f(150, 250));
    triangleD.setFillColor(transparentColor);
    triangleD.setOutlineColor(color);
    triangleD.setOutlineThickness(10);

    sf::RectangleShape rectD1(sf::Vector2f( 150, 10));
    rectD1.setFillColor(color);
    rectD1.setPosition(25, 250);

    sf::RectangleShape rectD2(sf::Vector2f( 10, 50));
    rectD2.setFillColor(color);
    rectD2.setPosition(15, 250);

    sf::RectangleShape rectD3(sf::Vector2f( 10, 50));
    rectD3.setFillColor(color);
    rectD3.setPosition(175, 250);
    /*Д*/

    /*К*/
    sf::RectangleShape rectK1(sf::Vector2f( 10, 230));
    rectK1.setFillColor(color);
    rectK1.setPosition(200, 70);
    sf::RectangleShape rectK2(sf::Vector2f( 10, 175));
    rectK2.setFillColor(color);
    rectK2.setPosition(325, 80);
    rectK2.setRotation(45);
    sf::RectangleShape rectK3(sf::Vector2f( 10, 125));
    rectK3.setFillColor(color);
    rectK3.setPosition(300, 280);
    rectK3.setRotation(135);
    /*К*/

    /*В*/
    auto circleB1 = createCircle(50, sf::Vector2f(350, 80), 30, color);
    auto circleB2 = createCircle(50, sf::Vector2f(350, 190), 30, color);
    /*В*/


    sf::RenderWindow window(sf::VideoMode(800, 600), "Dikov Kirill Vladimirovich");

    while (window.isOpen()) {
        sf::Event event;
        while (window.pollEvent(event)) {
            if (event.type == sf::Event::Closed) {
                window.close();
            }
        }


        window.clear(sf::Color::White);
        window.draw(triangleD);
        window.draw(rectD1);
        window.draw(rectD2);
        window.draw(rectD3);
        window.draw(rectK1);
        window.draw(rectK2);
        window.draw(rectK3);
        window.draw(circleB1);
        window.draw(circleB2);
        window.display();
    }
    return 0;
}
