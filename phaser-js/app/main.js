var game = new Phaser.Game(800, 500, Phaser.AUTO, '',
    {
        preload: preload,
        create: create,
        update: update,
        render: render
    });

function preload() {
    game.load.image('J', 'assets/J.png');
    game.load.image('K', 'assets/K.png');
    game.load.image('Q', 'assets/Q.png');
    game.load.image('T', 'assets/T.png');
}

const SYMBOL_WIDTH = 100,
      SYMBOL_HEIGHT = 100;

let world,
    symbols = [],
    symbolKeys = ['J', 'K', 'Q', 'T'],
    startPosition;
function create() {
    // game.stage.backgroundColor = '#2d2d2d';
    game.physics.startSystem(Phaser.Physics.ARCADE);
    world = game.world;

    startPosition = calcStartPosition(SYMBOL_WIDTH, world.width);
    //TODO: check world's the min size

    // all kinds of symbols must be present
    _.chain(symbolKeys)
        .shuffle()
        .each(key => addSymbol(symbols, key));

    let countOfSymbols = calcCountOfSymbols(SYMBOL_HEIGHT, world.height);
    while(symbols.length < countOfSymbols)
        addSymbol(symbols, getRandomItem(symbolKeys))
}



let isRunning = true;
let winKey;


function update() {
    //find the lower symbol
    let lowerSymbol = _.max(symbols, s => s.y);
    // let higherSymbol = _.min(symbols, s => s.y);

    //if at least part of the symbol is out of bounds and a new symbol with the same kye wasn't added in the top
    if(partOutOfBounds(lowerSymbol, world.height) && !lowerSymbol.replaced){
        //add a new symbol with the same kye in the top
        let newSymbol = createSymbol(startPosition.x, - SYMBOL_HEIGHT, lowerSymbol.key);
        symbols.push(newSymbol);
        log('add new symbol: ' + lowerSymbol.key, newSymbol)
        lowerSymbol.replaced = true;
    } else if(wholeOutOfBounds(lowerSymbol, world.height)){
        //delete the symbol if it's out of bounds
        let lowerSymbolIndex = _.chain(symbols)
            .pluck('key')
            .indexOf(lowerSymbol.key)
            .value();

        log('delete: ' + lowerSymbol.key)
        symbols.splice(lowerSymbolIndex, 1);
        lowerSymbol.kill();
    }
}

function calcStartPosition(symbolWidth, worldWidth){
    return {
        x: worldWidth - symbolWidth,
        y: 0
    };
}

function getRandomItem(items){
    let index = _.random(0, items.length - 1);
    return items[index];
}

function calcCountOfSymbols(symbolHeight, worldHeight){
    let count = worldHeight / symbolHeight;

    return Math.ceil(count.toPrecision(4) * 1);
}

function stopWhenWinSymbolInTheCenter(symbols, yCenter, winKey){
    log('stopping...')

    let winSymbol = findWinSymbol();
    if(isInTheCenter(winSymbol)){
        stop(symbols);
        return winSymbol;
    }

    function findWinSymbol(){
        return _.chain(symbols)
            .where({key: winKey})
            .first()
            .value();
    }

    function isInTheCenter(symbol){
        return center(symbol) === yCenter;
    }
}

function center(symbol){
    return Math.round(symbol.y + symbol.height / 2);
}

function stop(symbols){
    log('stopped')
    _.each(symbols, s => s.body.velocity.y = 0);
    return symbols;
}

function single(symbols, symbol){
    return _.where(symbols, {key: symbol.key}).length === 1;
}

function partOutOfBounds(symbol, worldHeight){
    return symbol.y + symbol.height >= worldHeight;
}

function wholeOutOfBounds(symbol, worldHeight){
    return symbol.y > worldHeight;
}

function render(){
    // game.debug.inputInfo(32, 32);
    // game.debug.spriteInputInfo(j, 32, 130);
    // game.debug.pointer( game.input.activePointer );
}



// var sprite1 = game.add.sprite(700, 0, 'J');
// var sprite2 = game.add.sprite(0, 0, 'K').alignTo(sprite1, Phaser.BOTTOM_RIGHT, 0);
// var sprite3 = game.add.sprite(0, 0, 'Q').alignTo(sprite2, Phaser.BOTTOM_RIGHT, 0);
// var sprite4 = game.add.sprite(0, 0, 'T').alignTo(sprite3, Phaser.BOTTOM_RIGHT, 0);
// var sprite5 = game.add.sprite(0, 0, 'T').alignTo(sprite4, Phaser.BOTTOM_RIGHT, 0);
// var sprite6 = game.add.sprite(0, 0, 'T').alignTo(sprite5, Phaser.BOTTOM_RIGHT, 0);




//
// var sprite1 = game.add.sprite(120, 250, 'J');
// var sprite2 = game.add.sprite(0, 0, 'K').alignTo(sprite1, Phaser.RIGHT_CENTER, 16);
// var sprite3 = game.add.sprite(0, 0, 'Q').alignTo(sprite2, Phaser.RIGHT_CENTER, 16);
// var sprite4 = game.add.sprite(0, 0, 'T').alignTo(sprite3, Phaser.RIGHT_CENTER, 16);
// var sprite5 = game.add.sprite(0, 0, 'T').alignTo(sprite4, Phaser.RIGHT_CENTER, 16);
// var sprite6 = game.add.sprite(0, 0, 'T').alignTo(sprite5, Phaser.RIGHT_CENTER, 16);

// symbols.push(createSymbol(0, 0, 'J'));
// symbols.push(createSymbol(0, 100, 'K'));
// symbols.push(createSymbol(0, 200, 'Q'));
// symbols.push(createSymbol(0, 300, 'T'));


// _.each(symbols, createSymbol);

// _.delay(() => isRunning = false, 5000);

// _.times(symbols.length, idx => {
//     let symbol = symbols[idx];
//     if (!symbol) return;
//
//     if (partOutOfBounds(symbol, world.height) && single(symbols, symbol)) {
//         symbols.push(createSymbol(0, -symbol.height, symbol.key));
//     } else if (wholeOutOfBounds(symbol, world.height)) {
//         let removeIdx = _.chain(symbols)
//             .pluck('key')
//             .indexOf(symbol.key)
//             .value();
//
//         symbols.splice(removeIdx, 1);
//
//         symbol.kill();
//     }
// });
//
// if (!isRunning && !winKey) {
//     winKey = stopWhenWinSymbolInTheCenter(symbols, world.centerY, 'Q');
// }