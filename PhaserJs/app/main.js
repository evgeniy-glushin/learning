var game = new Phaser.Game(800, 600, Phaser.AUTO, '', {preload: preload, create: create, update: update});

function preload() {
    game.load.image('sky', 'images/sky.png');
    game.load.image('ground', 'images/platform.png');
    game.load.image('star', 'images/star.png');
    game.load.spritesheet('dude', 'images/dude.png', 32, 48);
}

let platforms, player, cursors, stars, score = 0, scoreText;
function create() {
    game.physics.startSystem(Phaser.Physics.ARCADE);

    game.add.sprite(0, 0, 'sky');

    platforms = game.add.group();

    platforms.enableBody = true;

    let ground = platforms.create(0, game.world.height - 64, 'ground');
    ground.scale.setTo(2, 2);
    ground.body.immovable = true;

    let ledge = platforms.create(400, 400, 'ground');
    ledge.body.immovable = true;

    ledge = platforms.create(-150, 250, 'ground');
    ledge.body.immovable = true;

    player = game.add.sprite(32, game.world.height - 150, 'dude');

    game.physics.arcade.enable(player);

    //  Player physics properties. Give the little guy a slight bounce.
    player.body.bounce.y = 0.2;
    player.body.gravity.y = 300;
    player.body.collideWorldBounds = true;

    //  Our two animations, walking left and right.
    player.animations.add('left', [0, 1, 2, 3], 10, true);
    player.animations.add('right', [5, 6, 7, 8], 10, true);
    player.body.gravity.y = 300;

    cursors = game.input.keyboard.createCursorKeys();


    stars = game.add.group();

    stars.enableBody = true;

    //  Here we'll create 12 of them evenly spaced apart
    _.times(12, i => {
        //  Create a star inside of the 'stars' group
        var star = stars.create(i * 70, 0, 'star');

        //  Let gravity do its thing
        star.body.gravity.y = 6;

        //  This just gives each star a slightly random bounce value
        star.body.bounce.y = 0.7 + _.random() * 0.2;
    });

    scoreText = game.add.text(16, 16, 'score: 0', { fontSize: '32px', fill: '#000' });
}

function update() {
    let arcade = game.physics.arcade;
    //  Collide the player and the stars with the platforms
    var hitPlatform = arcade.collide(player, platforms);

    player.body.velocity.x = 0;

    if (cursors.left.isDown)
    {
        //  Move to the left
        player.body.velocity.x = -150;

        player.animations.play('left');
    }
    else if (cursors.right.isDown)
    {
        //  Move to the right
        player.body.velocity.x = 150;

        player.animations.play('right');
    }
    else
    {
        //  Stand still
        player.animations.stop();

        player.frame = 4;
    }

    //  Allow the player to jump if they are touching the ground.
    if (cursors.up.isDown && player.body.touching.down && hitPlatform)
    {
        player.body.velocity.y = -350;
    }


    game.physics.arcade.collide(stars, platforms);
    //As well as doing this we will also check to see if the player overlaps with a star or not:
    game.physics.arcade.overlap(player, stars, collectStar, null, this)
}

function collectStar(player, star) {
    // Removes the star from the screen
    star.kill();

    //  Add and update the score
    score += 10;
    scoreText.text = 'Score: ' + score;
}

// let game = new Phaser.Game(400, 270, Phaser.AUTO);
//
// let state = {
//     preload: function () {
//         this.load.image('background', 'images/background.jpg');
//         this.load.image('ninja', 'images/ninja.jpg');
//     },
//     create: function () {
//         var g = this.game;
//
//         this.background = g.add.sprite(0, 0, 'background');
//
//         this.ninja = g.add.sprite(g.world.centerX, g.world.centerY, 'ninja');
//         this.ninja.anchor.setTo(0.5);
//         this.ninja.scale.setTo(0.5);
//         this.ninja.scale.setTo(0.5, -0.5); //flip
//     },
//     update: function () {
//         this.ninja.angle += 0.5;
//     }
// };
//
// game.state.add('GameState', state);
// game.state.start('GameState');