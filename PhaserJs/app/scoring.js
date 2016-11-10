var game = new Phaser.Game(800, 600, Phaser.AUTO, '', {preload: preload, create: create, update: update});

function preload() {
    
}

let score,
    scoreBuffer;

function create() {
    game.stage.backgroundColor = '#16a085';

    //Keep track of the users score
    score = 0;
    scoreBuffer = 0;

    //Create the score label
    createScore();

    var seed = Date.now();
    var random = new Phaser.RandomDataGenerator([seed]);

    game.input.onUp.add(function(pointer){

        var newScore = random.integerInRange(1, 30);
        createScoreAnimation(pointer.x, pointer.y, '+'+newScore, newScore);

    }, game);
}

function update() {
    //While there is score in the score buffer, add it to the actual score
    if(scoreBuffer > 0){
        incrementScore();
        scoreBuffer--;
    }
}

let scoreLabel,
    scoreLabelTween;

function createScore () {
    var scoreFont = "100px Arial";

    //Create the score label
    scoreLabel = game.add.text(game.world.centerX, 50, "0", {font: scoreFont, fill: "#ffffff", stroke: "#535353", strokeThickness: 15});
    scoreLabel.anchor.setTo(0.5, 0);
    scoreLabel.align = 'center';

    //Create a tween to grow / shrink the score label
    scoreLabelTween = game.add.tween(scoreLabel.scale)
        .to({ x: 1.5, y: 1.5}, 200, Phaser.Easing.Linear.In)
        .to({ x: 1, y: 1}, 200, Phaser.Easing.Linear.In);
}

function incrementScore(){
    //Increase the score by one and update the total score label text
    score += 1;
    scoreLabel.text = score;
}

function createScoreAnimation (x, y, message, score){
    var scoreFont = "90px Arial";

    //Create a new label for the score
    var scoreAnimation = game.add.text(x, y, message, {font: scoreFont, fill: "#39d179", stroke: "#ffffff", strokeThickness: 15});
    scoreAnimation.anchor.setTo(0.5, 0);
    scoreAnimation.align = 'center';

    //Tween this score label to the total score label
    var scoreTween = game.add.tween(scoreAnimation).to({x: game.world.centerX, y: 50}, 800, Phaser.Easing.Exponential.In, true);

    //When the animation finishes, destroy this score label, trigger the total score labels animation and add the score
    scoreTween.onComplete.add(function(){
        scoreAnimation.destroy();
        scoreLabelTween.start();
        scoreBuffer += score;
    }, game);
}

