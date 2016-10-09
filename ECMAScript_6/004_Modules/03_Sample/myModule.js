'use strict';
// Экспорт пол умолчанию
// экспорт класса, для того чтобы получит доступ к данному классу в файле, где будет проводиться импорт не нужно использовать деструктуризацию
export default class MyObject {

    constructor(value) {
        this.value = value;
    }

    Method () {
        console.log("Value is " + this.value);
    }
}

// экспорт переменной
export let someValue = 100;

// экспорт функции
export let myLog = function (message) {
    console.log("My Log " + message);
}

// экспорт объекта
export let model = new MyObject("model Object");