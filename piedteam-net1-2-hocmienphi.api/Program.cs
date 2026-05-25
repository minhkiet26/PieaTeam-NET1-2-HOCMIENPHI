var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

//Kiến trúc 3 layer (Tầng)

// Tầng API
    // Chịu trách nhiệm khai báo các endpoint, 
        // nhận request, trả response
    // Config hệ thống
    // Tầng API gọi tới Service

// Tầng Service
    // Chịu trách nhiệm sử lý nghiệp vụ
    // Tương tác với tầng Repository để lấy dữ liệu
    // Tầng Service gọi tới Repository

// Tầng Repository
    // Chịu trách nhiệm tương tác với DB
    // Cấu hình những thứ liên quan tới DB
    
//Có 1 Req là đăng nhập vòa hệ thống
    //Tầng API: Muốn đăng nhập vào hệ thống
        // Chui vô đây: POST /api/auth/login,
            // Nhận request body {email: "tan", password: "123"}

    // Tầng API lúc này gọi xuống tầng Service có cái hàm là
        // Xử lí loginL LoginHandler(email, passowrd)
        // Lúc này hàm login trong Service hãy chạy như sau
            // Kiểm tra email || người dùng có tồn tại trong DB hay ko
            // người dùng này có bị banned hay ko
            // nếu có lỗi thì trả về lỗi
            // nếu ko có lỗi thì trả về Token đăng nhập

    // Tầng Service lúc này gọi xuống tầng Repository có cái hàm là 
        // GetUserByEmail(email)
        // Hàm này sẽ chạu câu lệnh SQL để
            // lấy thông tin người dùng ra khỏi DB

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}