"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var movies_service_1 = require("./movies.service");
describe('MoviesService', function () {
    beforeEach(function () { return testing_1.TestBed.configureTestingModule({}); });
    it('should be created', function () {
        var service = testing_1.TestBed.get(movies_service_1.MoviesService);
        expect(service).toBeTruthy();
    });
});
//# sourceMappingURL=movies.service.spec.js.map