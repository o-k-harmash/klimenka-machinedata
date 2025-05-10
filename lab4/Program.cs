// 1. Експериментальні дані (матриця результатів для кожної комбінації)
var experiment = new Experiment();
var experimentResults = experiment.Calculate(); // Y_ij

// 2. Розрахунок дисперсій по рядках (внутрішньогрупова дисперсія)
var rowDispersionCalculator = new Dispersion();
var rowDispersions = rowDispersionCalculator.Calculate(experimentResults); // S²_ij

// 3. Перевірка однорідності дисперсій за критерієм Кохрена
var cochranCriterion = new CochranCriterion();
cochranCriterion.Calculate(rowDispersions); // G

// 4. Розрахунок дисперсії відтворюваності
var reproducibilityDispersion = new ReproducibilityDispersion();
var reproducibilityDispersionResult = reproducibilityDispersion.Calculate(rowDispersions); // S²_y

// 5. Розрахунок середніх значень по кожній комбінації
var averageExperiment = new AverageExperiment();
var averageResponses = averageExperiment.Calculate(experimentResults); // Ȳ

// 6. Ручний розрахунок коефіцієнтів регресії в нормованих одиницях
var regressionCoefficientsCalc = new RegressionCoefficientsManual();
var normalizedCoefficients = regressionCoefficientsCalc.Calculate(averageResponses); // b0, b1, b2, b3

// 7. Денормалізація коефіцієнтів у фізичні одиниці
var denormalizer = new DenormalizeCoefficients();
var physicalCoefficients = denormalizer.Calculate(normalizedCoefficients); // a0, a1, a2, a3

// 8. Побудова прогнозованих значень ŷ для кожної комбінації
var predictor = new Predictor();
var predictedValues = predictor.Calculate(physicalCoefficients); // ŷ

// 9. Розрахунок середньоквадратичного відхилення (RMSE) між ŷ та Ȳ
var rmseCalculator = new RMSE();
var rmseValue = rmseCalculator.Calculate(averageResponses, predictedValues); // RMSE

// 10. Розрахунок критеріїв Стьюдента для коефіцієнтів
var studentCriteria = new StudentCriteria();
var tResults = studentCriteria.Calculate(rmseValue, normalizedCoefficients); // t_k

var dispersionOfAdequacy = new DispersionOfAdequacy();
var adequacyDispersion = dispersionOfAdequacy.Calculate(averageResponses, predictedValues); // S²_адв

var fCriteria = new FCriteria();
var fValue = fCriteria.Calculate(adequacyDispersion, reproducibilityDispersionResult); // F-критерій

// Звіт по кожному етапу
experiment.Report();
rowDispersionCalculator.Report();
cochranCriterion.Report();
reproducibilityDispersion.Report();
averageExperiment.Report();
regressionCoefficientsCalc.Report();
denormalizer.Report();
predictor.Report();
rmseCalculator.Report();
studentCriteria.Report();
dispersionOfAdequacy.Report();
fCriteria.Report();
