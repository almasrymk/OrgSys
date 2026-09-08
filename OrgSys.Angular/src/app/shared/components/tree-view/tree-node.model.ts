export interface TreeNode<T> {
  id: number;
  label: string;
  sublabel?: string;
  data: T;
  children: TreeNode<T>[];
}

/** Builds a TreeNode[] from any flat list carrying id/parentId, e.g. Account or Classification. */
export function buildTree<T>(
  items: T[],
  getId: (item: T) => number,
  getParentId: (item: T) => number,
  getLabel: (item: T) => string,
  getSublabel?: (item: T) => string | undefined,
): TreeNode<T>[] {
  const nodesById = new Map<number, TreeNode<T>>();
  const roots: TreeNode<T>[] = [];

  for (const item of items) {
    nodesById.set(getId(item), {
      id: getId(item),
      label: getLabel(item),
      sublabel: getSublabel?.(item),
      data: item,
      children: [],
    });
  }

  for (const item of items) {
    const node = nodesById.get(getId(item))!;
    const parent = nodesById.get(getParentId(item));
    if (parent) {
      parent.children.push(node);
    } else {
      roots.push(node);
    }
  }

  return roots;
}
