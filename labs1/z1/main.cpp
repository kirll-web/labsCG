#include <cmath>
#include <memory>
#include <SFML/Graphics.hpp>


//классы и функции с заглавной буквы
sf::ConvexShape createCircle(
    const float radius,
    const sf::Vector2f position,
    const int pointCount,
    const sf::Color color
) {
    sf::ConvexShape semiCircle;
    semiCircle.setPointCount(pointCount + 2);

    semiCircle.setPoint(0, sf::Vector2f(0, radius));

    for (int i = 0; i <= pointCount; ++i) {
        const float angle = (-90.0f + (i * 180.0f / pointCount)) * (3.14159265f / 180.0f);
        const float x = radius * std::cos(angle);
        const float y = radius * std::sin(angle) + radius;
        semiCircle.setPoint(i + 1, sf::Vector2f(x, y));
    }

    semiCircle.setOutlineColor(color);
    semiCircle.setOutlineThickness(10);
    semiCircle.setPosition(position);

    return semiCircle;
}

class K {
public:
    explicit K(sf::Color color) {
        sf::RectangleShape rectK1(sf::Vector2f(10, 230));
        rectK1.setFillColor(color);
        rectK1.setPosition(200, 70);
        sf::RectangleShape rectK2(sf::Vector2f(10, 175));
        rectK2.setFillColor(color);
        rectK2.setPosition(325, 80);
        rectK2.setRotation(45);
        sf::RectangleShape rectK3(sf::Vector2f(10, 125));
        rectK3.setFillColor(color);
        rectK3.setPosition(300, 280);
        rectK3.setRotation(135);

        mShapes.push_back(std::make_unique<sf::RectangleShape>(rectK1));
        mShapes.push_back(std::make_unique<sf::RectangleShape>(rectK2));
        mShapes.push_back(std::make_unique<sf::RectangleShape>(rectK3));
    }

    void draw(
        sf::RenderWindow &window,
        const float speed,
        const float deltaTime,
        float &direction,
        float &yOffset
    ) const {
        const float moveY = speed * deltaTime * direction;
        yOffset += moveY;

        // Меняем направление, если достигли границ (+50 вверх, обратно вниз)
        if (yOffset >= 150.0f || yOffset <= -10.0f) {
            direction *= -1;
        }

        for (auto &shape: mShapes) {
            shape->move(0, moveY);
            window.draw(*shape);
        }
    }

    ~K() {
        for (auto &shape: mShapes) {
            delete shape.get();
        }
    }

private:
    std::vector<std::unique_ptr<sf::Shape> > mShapes;
};

class D {
public:
    D(sf::Color color, sf::Color transparentColor) {
        /*Д*/
        sf::ConvexShape triangleD;
        triangleD.setPointCount(3);
        triangleD.setPoint(0, sf::Vector2f(100, 100));
        triangleD.setPoint(1, sf::Vector2f(50, 250));
        triangleD.setPoint(2, sf::Vector2f(150, 250));
        triangleD.setFillColor(transparentColor);
        triangleD.setOutlineColor(color);
        triangleD.setOutlineThickness(10);

        sf::RectangleShape rectD1(sf::Vector2f(150, 10));
        rectD1.setFillColor(color);
        rectD1.setPosition(25, 250);

        sf::RectangleShape rectD2(sf::Vector2f(10, 50));
        rectD2.setFillColor(color);
        rectD2.setPosition(15, 250);

        sf::RectangleShape rectD3(sf::Vector2f(10, 50));
        rectD3.setFillColor(color);
        rectD3.setPosition(175, 250);

        mShapes.push_back(std::make_unique<sf::RectangleShape>(rectD1));
        mShapes.push_back(std::make_unique<sf::RectangleShape>(rectD2));
        mShapes.push_back(std::make_unique<sf::RectangleShape>(rectD3));
        mShapes.push_back(std::make_unique<sf::ConvexShape>(triangleD));
        /*Д*/
    }

    void draw(sf::RenderWindow &window, const float speed, const float deltaTime, float &direction,
              float &yOffset) const {
        const float moveY = speed * deltaTime * direction;
        yOffset += moveY;

        if (yOffset >= 70.0f || yOffset <= 0.0f) {
            direction *= -1;
        }

        for (auto &shape: mShapes) {
            shape->move(0, moveY);
            window.draw(*shape);
        }
    }

    ~D() {
        for (auto &shape: mShapes) {
            delete shape.get();
        }
    }

private:
    std::vector<std::unique_ptr<sf::Shape> > mShapes;
};

class V {
public:
    explicit V(sf::Color color) {
        /*В*/
        auto circleB1 = createCircle(50, sf::Vector2f(350, 80), 30, color);
        auto circleB2 = createCircle(50, sf::Vector2f(350, 190), 30, color);
        /*В*/

        mShapes.push_back(std::make_unique<sf::ConvexShape>(circleB1));
        mShapes.push_back(std::make_unique<sf::ConvexShape>(circleB2));
        /*Д*/
    }


    void draw(sf::RenderWindow &window, const float deltaTime) {
        velocity += acceleration * deltaTime;
        yOffset += velocity * deltaTime;

        if (yOffset >= maxOffset || yOffset <= minOffset) {
            acceleration *= -1;
        }

        for (auto &shape: mShapes) {
            shape->move(0, velocity * deltaTime);
            window.draw(*shape);
        }
    }

    ~V() {
        for (auto &shape: mShapes) {
            delete shape.get();
        }
    }
private:
    std::vector<std::unique_ptr<sf::Shape>> mShapes;
    float yOffset = 0.0f;
    float velocity = 0.0f;
    float acceleration = 50.0f;
    const float minOffset = 0.0f;
    const float maxOffset = 50.0f;
};


int main() {
    auto color = sf::Color::Blue;
    auto transparentColor = sf::Color::Transparent;
    const auto d = D(color, transparentColor);
    const auto k = K(color);
    auto v = V(color);

//todo разобраться с равноускоренной скоростью
    sf::RenderWindow window(sf::VideoMode(800, 600), "Dikov Kirill Vladimirovich");
    const float speed = 100.0f;
    sf::Clock clock;
    float directionD = -1.0f;
    float directionK = -1.0f;
    float directionV = -1.0f;
    float yOffsetD = 3.0f;
    float yOffsetK = 10.0f;
    float yOffsetV = 30.0f; //todo перенести в классы
    while (window.isOpen()) {
        sf::Event event{};
        while (window.pollEvent(event)) {
            if (event.type == sf::Event::Closed) {
                window.close();
            }
        }

        const float deltaTime = clock.restart().asSeconds();
        window.clear(sf::Color::White);
        d.draw(window, speed, deltaTime, directionD, yOffsetD);
        k.draw(window, speed, deltaTime, directionK, yOffsetK);
        v.draw(window, deltaTime);
        window.display();
    }


    return 0;
}
