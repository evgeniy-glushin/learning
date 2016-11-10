var game = new Phaser.Game(800, 600, Phaser.AUTO, '', {preload: preload, create: create, update: update});

function preload() {

}

function create() {
    game.stage.backgroundColor = '#ffffff';
    // game.stage.backgroundColor = '#ef3d45';

    // var bmd = game.add.bitmapData(game.width, 100);
    // //
    // // //	Black and opaque
    // bmd.fill(0, 0, 0, 1);
    //
    // // bmd.addToWorld();
    //
    // //	Our text object
    // // let text = game.make.text(0, 0, "phaser", { font: "bold 32px Arial", fill: "#ff0044" });
    // // text.anchor.set(0.5);
    // //
    // let counter = 0;
    //
    // _.chain(_.range(0, 800, 100))
    //     .each(x => bmd.draw(game.make.text(0, 0, ++counter, { font: `bold ${32 + counter * 2}px Arial`, fill: "#ff0044" }), x, 0, null, null, 'destination-out'))
    //
    // // let str =  _.chain(_.range(0, 800, 100))
    // //     .join(' ')
    // //     .value()
    //
    //  // bmd.ctx.fillText('fsrgh', 0, 0)
    //
    // game.cache.addSpriteSheet('dynamic', '', bmd.canvas, 100, 100, 8, 0, 0);
    //
    // let numbers = game.add.sprite(100, 100, 'dynamic');
    // numbers.animations.add('float');
    //
    // numbers.play('float', 10, true);
    // game.add.image(0, 0, bmd);

    _.delay(() => animateWinScore(20), 0);
    // animateWinScore(20)
}

function animateWinScore (score) {
    const SIZE = 100;
    var bmd = game.add.bitmapData(SIZE * score, SIZE);

    //	Black and opaque
     bmd.fill(0,0,0,1);

     // bmd.addToWorld(200, 200);

    let scaleStep = 3 / score;
    let scale = 1;

    let animatedScore = 0;
    _.chain(_.range(0, score * SIZE, SIZE))
        .each(x => {
            // let txt = game.make.text(0, 0, ++animatedScore, { font: `bold 20px Arial`, fill: "#ff0044" });
            // txt.scale.set(scale += scaleStep);
            let txt = game.make.text(0, 0, ++animatedScore, { font: `bold ${32 + animatedScore * 2}px Arial`, fill: "#ff0044" });
            txt.anchor.setTo(0.5);
            bmd.draw(txt, x + 50, 50, null, null, 'destination-out');
        });

    game.cache.addSpriteSheet('dynamic', '', bmd.canvas, SIZE, SIZE, score, 0, 0);

    let numbers = game.add.sprite(200, 200, 'dynamic');
    let anim = numbers.animations.add('float');

    anim.onComplete.add((sprite, animation) => {
        numbers.kill();
    });

    numbers.play('float', 10, false);
    game.add.image(0, 0, bmd);
}

function update() {

}