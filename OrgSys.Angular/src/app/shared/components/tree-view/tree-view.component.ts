import { CommonModule } from '@angular/common';
import { Component, input, output } from '@angular/core';
import { TreeNode } from './tree-node.model';

export type TreeNodeAction = 'add-child' | 'edit' | 'delete';

/**
 * Generic recursive tree, replacing OrgSys.App's Areas/Setting/Views/Account/_AccountTreeNode.cshtml
 * partial. Reusable for any parent/child entity (Account, Classification, ...) — build the
 * TreeNode[] with `buildTree()` from tree-node.model.ts and supply it here.
 */
@Component({
  selector: 'app-tree-view',
  standalone: true,
  imports: [CommonModule, TreeViewComponent],
  templateUrl: './tree-view.component.html',
})
export class TreeViewComponent<T> {
  nodes = input.required<TreeNode<T>[]>();
  canEdit = input(true);
  canDelete = input(true);
  canAddChild = input(true);

  action = output<{ action: TreeNodeAction; node: TreeNode<T> }>();

  onAction(action: TreeNodeAction, node: TreeNode<T>): void {
    this.action.emit({ action, node });
  }

  onChildAction(event: { action: TreeNodeAction; node: TreeNode<T> }): void {
    this.action.emit(event);
  }
}
