# Distributed under the OSI-approved BSD 3-Clause License.  See accompanying
# file Copyright.txt or https://cmake.org/licensing for details.

cmake_minimum_required(VERSION 3.5)

# If CMAKE_DISABLE_SOURCE_CHANGES is set to true and the source directory is an
# existing directory in our source tree, calling file(MAKE_DIRECTORY) on it
# would cause a fatal error, even though it would be a no-op.
if(NOT EXISTS "X:/labsCG/lab3/z2/cmake-build-debug/_deps/glm-src")
  file(MAKE_DIRECTORY "X:/labsCG/lab3/z2/cmake-build-debug/_deps/glm-src")
endif()
file(MAKE_DIRECTORY
  "X:/labsCG/lab3/z2/cmake-build-debug/_deps/glm-build"
  "X:/labsCG/lab3/z2/cmake-build-debug/_deps/glm-subbuild/glm-populate-prefix"
  "X:/labsCG/lab3/z2/cmake-build-debug/_deps/glm-subbuild/glm-populate-prefix/tmp"
  "X:/labsCG/lab3/z2/cmake-build-debug/_deps/glm-subbuild/glm-populate-prefix/src/glm-populate-stamp"
  "X:/labsCG/lab3/z2/cmake-build-debug/_deps/glm-subbuild/glm-populate-prefix/src"
  "X:/labsCG/lab3/z2/cmake-build-debug/_deps/glm-subbuild/glm-populate-prefix/src/glm-populate-stamp"
)

set(configSubDirs )
foreach(subDir IN LISTS configSubDirs)
    file(MAKE_DIRECTORY "X:/labsCG/lab3/z2/cmake-build-debug/_deps/glm-subbuild/glm-populate-prefix/src/glm-populate-stamp/${subDir}")
endforeach()
if(cfgdir)
  file(MAKE_DIRECTORY "X:/labsCG/lab3/z2/cmake-build-debug/_deps/glm-subbuild/glm-populate-prefix/src/glm-populate-stamp${cfgdir}") # cfgdir has leading slash
endif()
