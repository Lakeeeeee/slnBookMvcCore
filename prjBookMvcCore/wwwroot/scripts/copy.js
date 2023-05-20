<script> //把page1複製到page3
    const check_btn = document.getElementById('check_btn');
    const table2 = document.getElementById('table2');

    check_btn.addEventListener('click', function () {
        const table1Rows = table1.querySelectorAll('tbody tr');
    // 清空 table2
    table2.innerHTML = '';

    // 创建 table2 的表头
    const table2Header = document.createElement('thead');
    const headerRow = document.createElement('tr');
    headerRow.innerHTML = `
    <th style="width:50px;"><input type="checkbox"></th>
    <th style="width:50px;">序號</th>
    <th style="width:500px;">書名</th>
    <th>優惠</th>
    <th>優惠價</th>
    <th>數量</th>
    <th>小計</th>
    `;
    table2Header.appendChild(headerRow);
    table2.appendChild(table2Header);

    // 复制 table1 的行到 table2
    table1Rows.forEach(function (row) {
            const quantityInput = row.querySelector('input[type="number"]');
    const clonedRow = row.cloneNode(true);

    // 更新复制的行的数量为调整后的值
    const clonedQuantityInput = clonedRow.querySelector('input[type="number"]');
    const clonedQuantityLabel = document.createElement('label');
    clonedQuantityLabel.textContent = quantityInput.value;
    clonedQuantityInput.parentNode.replaceChild(clonedQuantityLabel, clonedQuantityInput);

    table2.appendChild(clonedRow);
        });
    nextPage('nav-checkOutConfirm-tab');
    });
</script>
