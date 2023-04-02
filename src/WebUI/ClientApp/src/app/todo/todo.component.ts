import { Component, TemplateRef, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { fromEvent, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter, map } from 'rxjs/operators';
import {
  TodoTagsDto, TodoTagVm, CreateTodoTagsCommand, TodoTagsClient,
  TodoListsClient, TodoItemsClient, PaginatedListOfTodoItemSearchDto,
  TodoListDto, TodoItemDto, PriorityLevelDto,
  CreateTodoListCommand, UpdateTodoListCommand,
  CreateTodoItemCommand, UpdateTodoItemDetailCommand, TodoTagDto, UpdateTodoTagsCommand
} from '../web-api-client';

@Component({
  selector: 'app-todo-component',
  templateUrl: './todo.component.html',
  styleUrls: ['./todo.component.scss']
})
export class TodoComponent implements OnInit {
  debug = false;
  deleting = false;
  deleteCountDown = 0;
  deleteCountDownInterval: any;
  lists: TodoListDto[];
  priorityLevels: PriorityLevelDto[];
  selectedList: TodoListDto;
  selectedItem: TodoItemDto;
  tagLists: TodoTagsDto[];
  selectedTag: TodoTagDto;
  newListEditor: any = {};
  listOptionsEditor: any = {};
  newListModalRef: BsModalRef;
  listOptionsModalRef: BsModalRef;
  deleteListModalRef: BsModalRef;
  itemDetailsModalRef: BsModalRef;
  tagsModalRef: BsModalRef;
  tagEditor: any = {};
  color: string = '#FFFFFF';
  searchTitleChange = new Subject<string>();
  title: string;
  searching = true;
  selectedSort = null;
  itemDetailsFormGroup = this.fb.group({
    id: [null],
    listId: [null],
    priority: [''],
    note: [''],
    itemColour: [''],
    tagsId:[null]
  });

 tagsFormGroup = this.fb.group({
    id: [null],
   name: [''],
  });

  @ViewChild('searchInput', { static: true }) itemSearchInput!: ElementRef;

  constructor(
    private listsClient: TodoListsClient,
    private itemsClient: TodoItemsClient,
    private modalService: BsModalService,
    private tagsClient: TodoTagsClient,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.listsClient.get().subscribe(
      result => {
        this.lists = result.lists;
        this.priorityLevels = result.priorityLevels;
        this.tagLists = result.tags;
        if (this.lists.length) {
          this.selectedList = this.lists[0];
        }
      },
      error => console.error(error)
    );

    console.log(this.itemSearchInput);

    fromEvent(this.itemSearchInput.nativeElement, 'keyup').pipe(
      map((event: any) => {
        return event.target.value;
      }),filter(res => res.length > 2)
      ,debounceTime(1000)
      ,distinctUntilChanged()
    ).subscribe((text: string) => {
      this.searching = true;
      this.searchTodoItem(text);
    });
  }

  // Lists
  remainingItems(list: TodoListDto): number {
    return list.items.filter(t => !t.done).length;
  }

  showNewListModal(template: TemplateRef<any>): void {
    this.newListModalRef = this.modalService.show(template);
    setTimeout(() => document.getElementById('title').focus(), 250);
  }

  newListCancelled(): void {
    this.newListModalRef.hide();
    this.newListEditor = {};
  }

  updateListCancelled(): void {
    this.listOptionsModalRef.hide();
    this.newListEditor = {};
  }

  addList(): void {
    const list = {
      id: 0,
      title: this.newListEditor.title,
      items: []
    } as TodoListDto;

    this.listsClient.create(list as CreateTodoListCommand).subscribe(
      result => {
        list.id = result;
        this.lists.push(list);
        this.selectedList = list;
        this.newListModalRef.hide();
        this.newListEditor = {};
      },
      error => {
        const ex = JSON.parse(error.response);
        const titleNum = ex.errors.Title?.length || 0;
        if (ex || titleNum > 0) {
          this.newListEditor.error = ex.errors.Title[0];
        }

        setTimeout(() => document.getElementById('title').focus(), 250);
      }
    );
  }

  showListOptionsModal(template: TemplateRef<any>) {
    this.listOptionsEditor = {
      id: this.selectedList.id,
      title: this.selectedList.title
    };

    this.listOptionsModalRef = this.modalService.show(template);
  }

  updateListOptions() {
    const list = this.listOptionsEditor as UpdateTodoListCommand;
    this.listsClient.update(this.selectedList.id, list).subscribe(
      () => {
        (this.selectedList.title = this.listOptionsEditor.title),
          this.listOptionsModalRef.hide();
        this.listOptionsEditor = {};
      },
      error => {
        const ex = JSON.parse(error.response);
  
        const titleNum = ex.errors.Title?.length || 0;
        if (ex || titleNum > 0) {
          console.log(ex.errors.Title[0]);
          this.listOptionsEditor.error = ex.errors.Title[0];
        }
      }
    );
  }

  confirmDeleteList(template: TemplateRef<any>) {
    this.listOptionsModalRef.hide();
    this.deleteListModalRef = this.modalService.show(template);
  }

  deleteListConfirmed(): void {
    this.listsClient.delete(this.selectedList.id).subscribe(
      () => {
        this.deleteListModalRef.hide();
        this.lists = this.lists.filter(t => t.id !== this.selectedList.id);
        this.selectedList = this.lists.length ? this.lists[0] : null;
      },
      error => console.error(error)
    );
  }

  // Items
  showItemDetailsModal(template: TemplateRef<any>, item: TodoItemDto): void {
    this.selectedItem = item;
    this.itemDetailsFormGroup.patchValue(this.selectedItem);
    this.color = item.itemColour;
    this.itemDetailsModalRef = this.modalService.show(template);
    this.itemDetailsModalRef.onHidden.subscribe(() => {
        this.stopDeleteCountDown();
    });
  }

  updateItemDetails(): void {
    const item = new UpdateTodoItemDetailCommand(this.itemDetailsFormGroup.value);
    this.itemsClient.updateItemDetails(this.selectedItem.id, item).subscribe(
      () => {
        if (this.selectedItem.listId !== item.listId) {
          this.selectedList.items = this.selectedList.items.filter(
            i => i.id !== this.selectedItem.id
          );
          const listIndex = this.lists.findIndex(
            l => l.id === item.listId
          );
          this.selectedItem.listId = item.listId;
          this.lists[listIndex].items.push(this.selectedItem);
        }
        console.log(item);
        this.selectedItem.priority = item.priority;
        this.selectedItem.note = item.note;
        this.selectedItem.itemColour = item.itemColour;
        this.selectedItem.tagsId = item.tagsId;
        this.selectedItem.tags = this.tagLists.find(x => x.id === item.tagsId);
        this.itemDetailsModalRef.hide();
        this.itemDetailsFormGroup.reset();
      },
      error => console.error(error)
    );
  }

  public onChangeColor(itemColour: string): void {
    this.color = itemColour;
    this.itemDetailsFormGroup.patchValue({ itemColour });
  }

  public removeTag(): void {
    this.itemDetailsFormGroup.patchValue({tagsId:null });
  }


  addItem() {
    const item = {
      id: 0,
      listId: this.selectedList.id,
      priority: this.priorityLevels[0].value,
      title: '',
      done: false
    } as TodoItemDto;

    this.selectedList.items.push(item);
    const index = this.selectedList.items.length - 1;
    this.editItem(item, 'itemTitle' + index);
  }

  editItem(item: TodoItemDto, inputId: string): void {
    this.selectedItem = item;
    setTimeout(() => document.getElementById(inputId).focus(), 100);
  }

  updateItem(item: TodoItemDto, pressedEnter: boolean = false): void {
    const isNewItem = item.id === 0;

    if (!item.title.trim()) {
      this.deleteItem(item);
      return;
    }

    if (item.id === 0) {
      this.itemsClient
        .create({
          ...item, listId: this.selectedList.id
        } as CreateTodoItemCommand)
        .subscribe(
          result => {
            item.id = result;
          },
          error => console.error(error)
        );
    } else {
      this.itemsClient.update(item.id, item).subscribe(
        () => console.log('Update succeeded.'),
        error => console.error(error)
      );
    }

    this.selectedItem = null;

    if (isNewItem && pressedEnter) {
      setTimeout(() => this.addItem(), 250);
    }
  }

  deleteItem(item: TodoItemDto, countDown?: boolean) {
    if (countDown) {
      if (this.deleting) {
        this.stopDeleteCountDown();
        return;
      }
      this.deleteCountDown = 3;
      this.deleting = true;
      this.deleteCountDownInterval = setInterval(() => {
        if (this.deleting && --this.deleteCountDown <= 0) {
          this.deleteItem(item, false);
        }
      }, 1000);
      return;
    }
    this.deleting = false;
    if (this.itemDetailsModalRef) {
      this.itemDetailsModalRef.hide();
    }

    if (item.id === 0) {
      const itemIndex = this.selectedList.items.indexOf(this.selectedItem);
      this.selectedList.items.splice(itemIndex, 1);
    } else {
      this.itemsClient.delete(item.id).subscribe(
        () =>
        (this.selectedList.items = this.selectedList.items.filter(
          t => t.id !== item.id
        )),
        error => console.error(error)
      );
    }
  }

  stopDeleteCountDown() {
    clearInterval(this.deleteCountDownInterval);
    this.deleteCountDown = 0;
    this.deleting = false;
  }


  searchTodoItem(title: string){
    let id = this.selectedList.id;
    this.itemsClient.todoItemSearch(id, title, 1, 10).subscribe(
      result => {
        console.log(result);
        this.selectedList.items = result.items;
        this.searching = false;
      }, error => { console.error(error); this.searching = false; }
    )
  }
  //tags
  showTagsModal(template: TemplateRef<any>): void {
    this.tagsModalRef = this.modalService.show(template);
    this.tagsModalRef.onHidden.subscribe(() => {
      console.log("close");
    });
  }

  deleteTag(id: number) {

 
    if (id === 0) {
      this.tagLists.splice(this.tagLists.findIndex(a => a.id === id), 1)
    } else {
      this.tagsClient.delete(id).subscribe(
        () => {

          this.selectedList.items.forEach((item, index) => {
            if (item.tags?.id === id) {
              this.selectedList.items[index].tags = null;
            }
          });
          this.tagLists = this.tagLists.filter( t => t.id !== id)
        },
        error => console.error(error)
      );

    }


  }


  editTag(tag: TodoTagDto): void {
    this.selectedTag = tag;
    this.tagsFormGroup.setValue({
      id: tag.id,
      name: tag.name,
    })
  
  }

  
  addTag(): void {
    const tag = this.tagsFormGroup.value;

    this.tagsClient
      .create({
        ...tag
      } as CreateTodoTagsCommand)
      .subscribe(
        result => {
          tag.id = result;
          this.tagLists.push(tag);
          this.tagEditor = {};
        },
        error => {
          const ex = JSON.parse(error.response);
          const errorNum = ex.errors.Name?.length || 0;
          if (ex || errorNum > 0) {
            this.tagEditor.error = ex.errors.Name[0];
          }
        }
    );
    this.tagsFormGroup.reset();

  }

  updateTag(): void {
    const tag = new UpdateTodoTagsCommand(this.tagsFormGroup.value);

    if (!tag.name.trim()) {
      this.deleteTag(tag.id);
      return;
    }

    this.tagsClient.update(tag.id, tag).subscribe(
        () => { this.updateTaginTodoList(tag.id, tag) },
         error => {
          const ex = JSON.parse(error.response);
          const errorNum = ex.errors.Name?.length || 0;
          if (ex || errorNum > 0) {
            this.tagEditor.error = ex.errors.Name[0];
          }
        }
    );

  }

  updateTaginTodoList(id: number, tag: TodoTagDto): void {


    this.selectedList.items.forEach((item, index) => {
      if (item.tags?.id === id) {
        this.selectedList.items[index].tags = tag;
      }
    });

    this.tagLists.forEach((lisTag, index) => {
      if (lisTag?.id === id) {
        this.tagLists[index] = tag;
      }
    });

    this.tagEditor = {};
    this.selectedTag = null;
    this.tagsFormGroup.reset();
  }

  //filter
  filterItemByTag() {
    let sort = this.selectedSort;
    switch (sort) {
      case "1":
        this.orderMostUsedTag();
        break;
      case "2":
        this.orderLeastUsedTag();
        break;
    }
  }

  orderMostUsedTag() {
    this.tagsClient.get2(1).subscribe(
      (result) => {
        this.tagLists = result.todoTags
      },
      error => console.error(error)
    );
  }


  orderLeastUsedTag() {
    this.tagsClient.get2(2).subscribe(
      (result) => {
        this.tagLists = result.todoTags
      },
      error => console.error(error)
    );
  }

}
