## Подробности
DifferenceAll позволяет создать новое тело путем вычитания списка тел из одного тела. Входной элемент solid обозначает тело, из которого выполняется вычитание, а входной элемент tool — вычитаемый список тел. Тела в этом списке объединяются в одно тело, которое затем вычитается из входного элемента solid. В примере ниже в качестве тела, из которого выполняется вычитание, используется стандартный куб. Числовые регуляторы позволяют управлять положением и радиусом сферы. На основе последовательности чисел, используемой в качестве координаты z, создается список из нескольких сфер. Если сферы пересекают куб, то результат представляет собой куб, из которого вычтены части, пересекающиеся со сферами.
___
## Файл примера

![DifferenceAll](./Autodesk.DesignScript.Geometry.Solid.DifferenceAll_img.jpg)

