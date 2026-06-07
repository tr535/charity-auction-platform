using AutoMapper;
using Microsoft.EntityFrameworkCore;
using server.BLL.Interfaces;
using server.DAL;
using server.Models;
using server.Models.DTO;
using Microsoft.Extensions.Logging;

public class DonorService : IDonorService
{
    private readonly IDonorDal _donorDal;
    private readonly IMapper _mapper;
    private readonly ILogger<DonorService> _logger;

    public DonorService(IDonorDal donorDal, IMapper mapper, ILogger<DonorService> logger)
    {
        _donorDal = donorDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BaseResponseDTO<IEnumerable<DonorDTO>>> GetAllDonorsAsync(string? name, string? email)
    {
        _logger.LogInformation("שליפת כל התורמים. פילטרים: שם={Name}, אימייל={Email}", name ?? "ללא", email ?? "ללא");

        // תיקון השגיאה: משתמשים ב-Queryable שמגיע מה-DAL. 
        // ה-Include כבר נמצא בתוך ה-DAL ולכן אין צורך להוסיף אותו כאן.
        var query = _donorDal.GetDonorsQueryable();

        if (!string.IsNullOrEmpty(name))
            query = query.Where(d => (d.FirstName + " " + d.FamilyName).Contains(name));

        if (!string.IsNullOrEmpty(email))
            query = query.Where(d => d.Email.Contains(email));

        // המרה לרשימה בצורה אסינכרונית
        var donors = await query.ToListAsync();

        // מיפוי ל-DTO והחזרה
        return BaseResponseDTO<IEnumerable<DonorDTO>>.Ok(_mapper.Map<IEnumerable<DonorDTO>>(donors));
    }

    public async Task<BaseResponseDTO<DonorDTO>> GetDonorByIdAsync(int id)
    {
        // ה-Include כבר מובנה בתוך ה-DAL בשיטה זו
        var donor = await _donorDal.GetDonorByIdAsync(id);
        if (donor == null)
        {
            _logger.LogWarning("תורם עם מזהה {Id} לא נמצא בשליפה אישית", id);
            return BaseResponseDTO<DonorDTO>.Failure("תורם לא נמצא");
        }
        return BaseResponseDTO<DonorDTO>.Ok(_mapper.Map<DonorDTO>(donor));
    }

    public async Task<BaseResponseDTO<DonorDTO>> AddDonorAsync(DonorDTO donorDTO)
    {
        _logger.LogInformation("מנסה להוסיף תורם חדש למערכת: {Email}", donorDTO.Email);

        var donor = _mapper.Map<Donor>(donorDTO);
        await _donorDal.AddDonorAsync(donor);
        await _donorDal.SaveChangesAsync();

        _logger.LogInformation("תורם נוסף בהצלחה. מזהה חדש: {Id}", donor.Id);
        return BaseResponseDTO<DonorDTO>.Ok(_mapper.Map<DonorDTO>(donor), "תורם נוסף בהצלחה");
    }

    public async Task<BaseResponseDTO<bool>> UpdateDonorAsync(int id, DonorDTO donorDTO)
    {
        _logger.LogInformation("עדכון פרטי תורם מזהה {Id}", id);

        var existing = await _donorDal.GetDonorByIdAsync(id);
        if (existing == null)
        {
            _logger.LogWarning("ניסיון עדכון נכשל: תורם {Id} לא קיים", id);
            return BaseResponseDTO<bool>.Failure("תורם לא נמצא");
        }

        _mapper.Map(donorDTO, existing);
        await _donorDal.UpdateDonorAsync(existing);
        await _donorDal.SaveChangesAsync();

        _logger.LogInformation("פרטי תורם {Id} עודכנו בהצלחה", id);
        return BaseResponseDTO<bool>.Ok(true, "פרטי תורם עודכנו");
    }

    public async Task<BaseResponseDTO<bool>> RemoveDonorAsync(int id)
    {
        _logger.LogWarning("ניסיון הסרת תורם מזהה {Id}", id);

        var existing = await _donorDal.GetDonorByIdAsync(id);
        if (existing == null)
        {
            _logger.LogWarning("ניסיון הסרה נכשל: תורם {Id} לא נמצא", id);
            return BaseResponseDTO<bool>.Failure("תורם לא נמצא");
        }

        // בדיקה אם קיימות מתנות קשורות (נשמר בזכות ה-Include ב-DAL)
        if (existing.Gifts != null && existing.Gifts.Any())
        {
            _logger.LogError("חסימת מחיקה: לתורם {Id} יש {Count} מתנות קשורות", id, existing.Gifts.Count);
            return BaseResponseDTO<bool>.Failure("לא ניתן למחוק תורם הקשור למתנות");
        }

        await _donorDal.RemoveDonorAsync(id);
        await _donorDal.SaveChangesAsync();

        _logger.LogInformation("תורם {Id} הוסר בהצלחה מהמערכת", id);
        return BaseResponseDTO<bool>.Ok(true, "תורם הוסר בהצלחה");
    }
}