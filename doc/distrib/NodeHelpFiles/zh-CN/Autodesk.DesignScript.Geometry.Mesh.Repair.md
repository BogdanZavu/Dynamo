## 详细
返回修复以下缺陷的新网格:
- 小组件: 如果网格包含非常小(相对于整体网格大小)的断开连接的分段，它们将被丢弃。
- 孔: 填充网格中的孔。
- 非流形区域: 如果一个顶点连接到两个以上的*边界*边，或者一个边连接到两个以上的三角形，则顶点/边是非流形的。网格工具套件将移除几何图形，直到网格变为流形。
此方法尝试保留尽可能多的原始网格，而 MakeWatertight 则会对网格重新采样。

在下面的示例中，“Mesh.Repair”用于导入的网格，以填充兔子耳朵周围的孔。

## 示例文件

![Example](./Autodesk.DesignScript.Geometry.Mesh.Repair_img.jpg)
