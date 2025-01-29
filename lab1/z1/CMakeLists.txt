cmake_minimum_required(VERSION 3.25)
project(lab1)

include(FetchContent)
set(BUILD_SHARED_LIBS OFF)
FETCHCONTENT_DECLARE(SFML GIT_REPOSITORY https://github.com/SFML/SFML.git GIT_TAG 2.5.1)
FETCHCONTENT_MAKEAVAILABLE(SFML)

set(CMAKE_CXX_STANDARD 17)

add_executable(lab1 main.cpp
        Model.cpp
        Model.h
        presenter.cpp
        presenter.h)

target_link_libraries(lab1 sfml-graphics)
target_include_directories(lab1 PRIVATE ${SFML_SOURCE_DIR}/include)

