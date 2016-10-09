'use strict';

// Именованый экспорт
// Класс MyObject будет доступен в коде где произойдет импорт файла myModule.js

export class MyObject {

    constructor(value) {
        this.value = value;
    }

    Method () {
        _privateMethod(this.value);
    }
}

function _privateMethod (value) {
    console.log("privateMethod " + value);
}