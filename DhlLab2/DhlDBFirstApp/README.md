# DhlDBFirstApp (Database First)

Project riêng cho phần Database First của Lab 4. Các entity và `DhlSchoolContext` trong `Models/` được viết theo schema `DhlSchool` trong lab; `LearnersController` và các Razor views cung cấp CRUD cho Learner.

## Chạy ứng dụng

Database `DhlSchool` phải tồn tại và có các bảng `Learner`, `Major`, `Course`, `Enrollment`. Đặt connection string trên máy bằng User Secrets hoặc biến môi trường, không commit mật khẩu vào source:

```powershell
dotnet user-secrets init --project DhlDBFirstApp
dotnet user-secrets set "ConnectionStrings:DhlSchoolContext" "Server=YOUR_INSTANCE;Database=DhlSchool;Trusted_Connection=True;TrustServerCertificate=True" --project DhlDBFirstApp
dotnet run --project DhlDBFirstApp
```

## Scaffold lại trực tiếp từ SQL Server

Khi database sẵn sàng, có thể sinh lại entity/context bằng EF Core CLI:

```powershell
dotnet ef dbcontext scaffold "YOUR_CONNECTION_STRING" Microsoft.EntityFrameworkCore.SqlServer --project DhlDBFirstApp --startup-project DhlDBFirstApp --context DhlSchoolContext --context-dir Models --output-dir Models --table Learner --table Major --table Course --table Enrollment --no-onconfiguring --force
```

Lệnh `--force` ghi đè model/context scaffold trong `Models/`; hãy giữ bản hiện tại nếu đã chỉnh sửa thủ công.
