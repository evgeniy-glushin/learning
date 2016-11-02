describe('calculator', function(){
    let Calculator = require('../lib/calculator');
    let calc;

    beforeEach(()=>{
        calc = new Calculator();
    })

    it('should add two numbers', () => {
        expect(calc.add(2, 3)).toEqual(5);
    })


    it('should multiply two numbers', () => {
        expect(calc.mul(2, 3)).toEqual(6);
    })
})