## Подробности
`Mesh.Remesh` создает сеть, в которой треугольники заданного объекта перераспределяются более равномерно, независимо от изменения их нормалей. Эта операция может пригодиться, когда необходимо подготовить сеть с переменной плотностью треугольников к анализу прочности. При многократной регенерации сети она получается все более однородной. Для сетей, вершины которых уже равноудалены друг от друга (например, икосферных сетей), результатом использования `Mesh.Remesh` будет та же сеть.
В приведенном ниже примере `Mesh.Remesh` используется для импортированной сети с высокой плотностью треугольников в областях с точной детализацией. Результат использования узла `Mesh.Remesh` перемещается в сторону, а для визуализации результата используется узел `Mesh.Edges`.

`(Используемый файл примера доступен по лицензии Creative Commons)`

## Файл примера

![Example](./Autodesk.DesignScript.Geometry.Mesh.Remesh_img.jpg)
